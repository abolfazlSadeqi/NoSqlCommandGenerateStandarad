using Core;
using Newtonsoft.Json;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services;

public class MongoScriptService : IMongoScriptService
{
    public string GenerateListDatabasesScript()
    {
        return "show dbs;";
    }
    public string GenerateUseDatabaseScript(string dbName)
    {
        if (string.IsNullOrWhiteSpace(dbName))
            return "// Database name is empty";

        return $"use {dbName}";
    }

    public string GenerateDropDatabaseScript(string dbName, bool forceDelete)
    {
        if (string.IsNullOrWhiteSpace(dbName))
            return "// Database name is empty";

        string script = "";

        if (forceDelete)
        {
            script += $"// Checks if database exists and drops it\n";
            script += $"db.getMongo().getDBNames().indexOf('{dbName}') >= 0 && db.getSiblingDB('{dbName}').dropDatabase();\n";
        }
        else
        {
            script += $"// Drop database WITHOUT existence check\n";
            script += $"db.getSiblingDB('{dbName}').dropDatabase();\n";
        }

        return script;
    }

    public string GenerateCreateDatabaseScript(MongoDatabaseCreateViewModel model)
    {
        var script = new StringBuilder();

        script.AppendLine($"use {model.DatabaseName}");
        if (model.UseCollation is not null && model.UseCollation.Value==true)

        {
            script.AppendLine();
            script.AppendLine($"db.createCollection(\"init\", {{ collation: {{ locale: \"{model.CollationLocale}\", strength: {model.CollationStrength} }} }});");
            script.AppendLine("db.init.drop();");
        }

        if (model.CreateUser is not null && model.CreateUser.Value == true)
        {
            script.AppendLine();
            script.AppendLine("db.createUser({");
            script.AppendLine($"  user: \"{model.Username}\",");
            script.AppendLine($"  pwd: \"{model.Password}\",");
            script.AppendLine($"  roles: [{{ role: \"{model.UserRole}\", db: \"{model.DatabaseName}\" }}]");
            script.AppendLine("});");
        }
        if (model.EnableSharding is not null && model.EnableSharding.Value == true)
       
        {
            script.AppendLine();
            script.AppendLine($"sh.enableSharding(\"{model.DatabaseName}\");");
        }
        if (model.UseWriteConcern is not null && model.UseWriteConcern.Value == true)
        
        {
            script.AppendLine();
            script.AppendLine($"db.getMongo().setWriteConcern({{ w: \"{model.W}\", j: {model.J?.ToString().ToLower()}, wtimeout: {model.WTimeout} }});");
        }
        if (model.UseReadConcern is not null && model.UseReadConcern.Value == true)
         
        {
            script.AppendLine();
            script.AppendLine($"db.getMongo().setReadConcern(\"{model.ReadConcern}\");");
        }
        if (model.CreateDefaultCollection is not null && model.CreateDefaultCollection.Value == true)
        {
            script.AppendLine();
            script.AppendLine($"db.createCollection(\"{model.CollectionName}\");");
        }

        return script.ToString();
    }


}

public class MongoCollectionScriptService : IMongoCollectionScriptService
{

    public string GenerateCreateCollectionScript(MongoCreateCollectionViewModel m)
    {
        var options = new List<string>();

        // Capped
        if (m.IsCapped)
        {
            var capped = "\"capped\": true";
            if (m.CappedSize.HasValue)
                capped += $", \"size\": {m.CappedSize.Value}";
            if (m.CappedMax.HasValue)
                capped += $", \"max\": {m.CappedMax.Value}";

            options.Add(capped);
        }

        // Collation
        if (m.UseCollation)
        {
            options.Add($"\"collation\": {{ \"locale\": \"{m.CollationLocale}\", \"strength\": {m.CollationStrength ?? 1} }}");
        }

        // Validation
        if (m.UseValidation && !string.IsNullOrWhiteSpace(m.ValidationRuleJson))
        {
            options.Add($"\"validator\": {m.ValidationRuleJson}");
            if (!string.IsNullOrWhiteSpace(m.ValidationLevel))
                options.Add($"\"validationLevel\": \"{m.ValidationLevel}\"");
        }

        // Comment
        if (!string.IsNullOrWhiteSpace(m.Comment))
            options.Add($"\"comment\": \"{m.Comment}\"");

        // Write Concern
        if (m.UseWriteConcern)
        {
            var wcList = new List<string>();
            if (m.WriteConcernW.HasValue)
                wcList.Add($"\"w\": {m.WriteConcernW}");
            if (m.WriteConcernJ.HasValue)
                wcList.Add($"\"j\": {m.WriteConcernJ.ToString().ToLower()}");

            options.Add($"\"writeConcern\": {{ {string.Join(", ", wcList)} }}");
        }

        // TimeSeries
        if (m.UseTimeSeries)
        {
            var ts = $"\"timeSeries\": {{ \"timeField\": \"{m.TimeSeriesTimeField}\"";
            if (!string.IsNullOrWhiteSpace(m.TimeSeriesMetaField))
                ts += $", \"metaField\": \"{m.TimeSeriesMetaField}\"";
            ts += " }";

            options.Add(ts);
        }

        string finalOptions = options.Count > 0
            ? "{ " + string.Join(", ", options) + " }"
            : "{}";

        return
    $@"use {m.DatabaseName}

db.createCollection(""{m.CollectionName}"", 
{finalOptions}
);";
    }
    public string GenerateDropCollectionScript(MongoDropCollectionViewModel m)
    {
        if (string.IsNullOrWhiteSpace(m.DatabaseName) || string.IsNullOrWhiteSpace(m.CollectionName))
            return "// Database or Collection name is empty";

        return
    $@"use {m.DatabaseName}

db.{m.CollectionName}.drop();";
    }
    public string GenerateShowCollectionsScript(MongoListCollectionsViewModel m)
    {
        if (string.IsNullOrWhiteSpace(m.DatabaseName))
            return "// Database name is required";

        if (string.IsNullOrWhiteSpace(m.SearchKeyword))
        {
            return
    $@"use {m.DatabaseName}

show collections;";
        }
        else
        {
            return
    $@"use {m.DatabaseName}

db.getCollectionNames().filter(c => c.includes(""{m.SearchKeyword}""));";
        }
    }

    public string GenerateRenameCollectionScript(MongoRenameCollectionViewModel m)
    {
        if (string.IsNullOrWhiteSpace(m.DatabaseName) ||
            string.IsNullOrWhiteSpace(m.OldCollectionName) ||
            string.IsNullOrWhiteSpace(m.NewCollectionName))
            return "// Database or Collection names are missing";

        return
    $@"use {m.DatabaseName}

db.{m.OldCollectionName}.renameCollection(""{m.NewCollectionName}"");";
    }


}
public class MongoIndexScriptService : IMongoIndexScriptService
{
    public string AdvanceIndexGenerateScript(AdvancedIndexViewModel vm)
    {
        string db = vm.DatabaseName;
        string col = vm.CollectionName;

        var indexDoc = new Dictionary<string, object>();

        // wildcard
        if (vm.IsWildcard)
        {
            indexDoc["$**"] = 1;
        }
        else
        {
            foreach (var f in vm.Fields)
            {
                if (vm.IsText)
                    indexDoc[f.FieldName] = "text";
                else if (vm.IsHashed)
                    indexDoc[f.FieldName] = "hashed";
                else
                    indexDoc[f.FieldName] = int.Parse(f.Order);
            }
        }

        string body = JsonConvert.SerializeObject(indexDoc, Newtonsoft.Json.Formatting.Indented);
        string options = "{\n";

        if (vm.IsUnique) options += "  unique: true,\n";
        if (vm.IsSparse) options += "  sparse: true,\n";
        if (vm.IsHidden) options += "  hidden: true,\n";
        if (vm.IsTTL) options += $"  expireAfterSeconds: {vm.TTLSeconds},\n";

        if (!string.IsNullOrWhiteSpace(vm.PartialFilter))
            options += $"  partialFilterExpression: {vm.PartialFilter},\n";

        if (!string.IsNullOrWhiteSpace(vm.Collation))
            options += $"  collation: {vm.Collation},\n";

        if (!string.IsNullOrWhiteSpace(vm.CustomIndexName))
            options += $"  name: \"{vm.CustomIndexName}\",\n";

        if (options.EndsWith(",\n"))
            options = options.Substring(0, options.Length - 2);

        options += "\n}";

        return
$@"use {db};

db.{col}.createIndex(
{body},
{options}
);";
    }
    public string GenerateCreateIndexScript(MongoCreateIndexViewModel m)
    {
        if (string.IsNullOrWhiteSpace(m.DatabaseName) || string.IsNullOrWhiteSpace(m.CollectionName) || m.Fields.Count == 0)
            return "// Database, Collection or Fields missing";

        var indexFields = new List<string>();
        foreach (var f in m.Fields)
        {
            var sort = f.SortOrder == "-1" ? -1 : 1;
            indexFields.Add($"\"{f.FieldName}\": {sort}");
        }

        var options = new List<string>();
        if (!string.IsNullOrWhiteSpace(m.IndexName))
            options.Add($"name: \"{m.IndexName}\"");
        if (m.Unique) options.Add("unique: true");
        if (m.Sparse) options.Add("sparse: true");
        if (m.ExpireAfterSeconds.HasValue) options.Add($"expireAfterSeconds: {m.ExpireAfterSeconds.Value}");
        if (m.Background) options.Add("background: true");
        if (!string.IsNullOrWhiteSpace(m.CollationLocale))
        {
            options.Add($"collation: {{ locale: \"{m.CollationLocale}\", strength: {m.CollationStrength ?? 1} }}");
        }
        if (!string.IsNullOrWhiteSpace(m.PartialFilterExpression))
            options.Add($"partialFilterExpression: {m.PartialFilterExpression}");
        if (!string.IsNullOrWhiteSpace(m.Comment))
            options.Add($"comment: \"{m.Comment}\"");

        string optionsText = options.Count > 0 ? "{ " + string.Join(", ", options) + " }" : "{}";

        return
    $@"use {m.DatabaseName}

db.{m.CollectionName}.createIndex(
    {{ {string.Join(", ", indexFields)} }},
    {optionsText}
);";
    }
    public string GenerateDropIndexScript(MongoDropIndexViewModel m)
    {
        if (string.IsNullOrWhiteSpace(m.DatabaseName) ||
            string.IsNullOrWhiteSpace(m.CollectionName) ||
            string.IsNullOrWhiteSpace(m.IndexName))
            return "// Database, Collection or Index name is missing";

        return
    $@"use {m.DatabaseName}

db.{m.CollectionName}.dropIndex(""{m.IndexName}"");";
    }
    // Generates a MongoDB command to list indexes of a collection
    public string GenerateListIndexesScript(string database, string collection)
    {
        if (string.IsNullOrWhiteSpace(database) || string.IsNullOrWhiteSpace(collection))
            return "// Provide both database and collection name";

        return $"use {database};\ndb.getCollection(\"{collection}\").getIndexes();";
    }

    // Optional: format Index info (mocked for UI)
    public List<string> MockIndexes(string collection)
    {
        return new List<string>
        {
            $"Index: _id_ on {collection} (default primary key)",
            $"Index: idx_name on {collection} (field: name, unique: false)"
        };
    }

}

public class MongoInfoService : IMongoInfoService
{
    public string GenerateGeneralCommandScript(MongoGeneralCommandViewModel m)
    {
        switch (m.CommandType)
        {
            case "Ping":
                return "db.runCommand({ ping: 1 });";
            case "ServerStatus":
                return "db.serverStatus();";
            case "BuildInfo":
                return "db.runCommand({ buildInfo: 1 });";
            case "CurrentOp":
                return "db.currentOp();";
            case "ReplicaSetStatus":
                return "rs.status();";
            case "ShardingStatus":
                return "sh.status();";
            case "ListDatabases":
                return "show dbs;";
            case "UseDatabase":
                return string.IsNullOrWhiteSpace(m.DatabaseName) ? "// DatabaseName required" : $"use {m.DatabaseName}";
            case "DropDatabase":
                return string.IsNullOrWhiteSpace(m.DatabaseName) ? "// DatabaseName required" : $"use {m.DatabaseName}\ndb.dropDatabase();";
            case "ListUsers":
                return "db.getUsers();";
            case "ListRoles":
                return "db.getRoles({ showPrivileges: true });";
            default:
                return "// Command not implemented";
        }
    }
}
public class MongoSecurityService : IMongoSecurityService
{

    public string GenerateUserScript(MongoUserManagementViewModel m)
    {
        switch (m.ActionType)
        {
            case "CreateUser":
                return $"db.createUser({{\n  user: \"{m.UserName}\",\n  pwd: \"{m.Password}\",\n  roles: [{string.Join(",", m.SelectedRoles.Select(r => "{ role: \"" + r + "\", db: \"" + m.DatabaseName + "\" }"))}]\n}});";
            case "DropUser":
                return $"db.dropUser(\"{m.UserName}\");";
            case "UpdatePassword":
                return $"db.updateUser(\"{m.UserName}\", {{ pwd: \"{m.Password}\" }});";
            case "GrantRole":
                return $"db.grantRolesToUser(\"{m.UserName}\", [{string.Join(",", m.SelectedRoles.Select(r => "{ role: \"" + r + "\", db: \"" + m.DatabaseName + "\" }"))}]);";
            case "RevokeRole":
                return $"db.revokeRolesFromUser(\"{m.UserName}\", [{string.Join(",", m.SelectedRoles.Select(r => "{ role: \"" + r + "\", db: \"" + m.DatabaseName + "\" }"))}]);";
            case "ListUsers":
                return "db.getUsers();";
            default:
                return "// Action not implemented";
        }
    }

    public string GenerateRoleScript(MongoRoleManagementViewModel m)
    {
        switch (m.ActionType)
        {
            case "CreateRole":
                return $"db.createRole({{\n  role: \"{m.RoleName}\",\n  privileges: {m.PrivilegesJson ?? "[]"},\n  roles: [{string.Join(",", m.ParentRoles.Select(r => "\"" + r + "\""))}]\n}});";
            case "DropRole":
                return $"db.dropRole(\"{m.RoleName}\");";
            case "AddPrivilege":
                return $"db.grantPrivilegesToRole(\"{m.RoleName}\", {m.PrivilegesJson});";
            case "RevokePrivilege":
                return $"db.revokePrivilegesFromRole(\"{m.RoleName}\", {m.PrivilegesJson});";
            case "ListRoles":
                return "db.getRoles({ showPrivileges: true });";
            default:
                return "// Action not implemented";
        }
    }

}

public class MongoDocumentService : IMongoDocumentService
{
    // Generates MongoDB insert script
    public string GenerateInsertScript(MongoDocumentInsertViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.DatabaseName) || string.IsNullOrWhiteSpace(model.CollectionName) || string.IsNullOrWhiteSpace(model.DocumentJson))
            return "// Please provide Database, Collection and Document JSON";

        string command = $"use {model.DatabaseName}\n";

        if (model.IsMultiple)
        {
            command += $"db.getCollection(\"{model.CollectionName}\").insertMany({model.DocumentJson}";
        }
        else
        {
            command += $"db.getCollection(\"{model.CollectionName}\").insertOne({model.DocumentJson}";
        }

        if (!string.IsNullOrWhiteSpace(model.WriteConcern))
        {
            command += $", {{ writeConcern: {{ w: \"{model.WriteConcern}\" }} }}";
        }

        command += ");";
        return command;
    }

  public string GenerateUpdateScript(MongoDocumentUpdateViewModel model)
{
    if (string.IsNullOrWhiteSpace(model.DatabaseName) || 
        string.IsNullOrWhiteSpace(model.CollectionName) ||
        string.IsNullOrWhiteSpace(model.FilterJson) || 
        string.IsNullOrWhiteSpace(model.UpdateJson))
    {
        return "// Provide Database, Collection, Filter JSON, and Update JSON";
    }

    string updateJson = model.UpdateJson.Trim();

    // --- FIX #1: Prevent missing atomic operator -------------------
    if (!updateJson.StartsWith("{"))
        updateJson = "{" + updateJson + "}";

    // لیست اپراتورهای معتبر MongoDB
    var atomicOps = new[] { "$set", "$inc", "$push", "$pull", "$unset", "$rename", "$addToSet", "$mul" };

    bool containsOperator = atomicOps.Any(op => updateJson.Contains($"\"{op}\""));

    // اگر کاربر Operator نداد → خودکار $set
    if (!containsOperator)
    {
        updateJson = "{ \"$set\": " + updateJson + " }";
    }

    // ----------------------------------------------------------------

    string command = $"use {model.DatabaseName}\n";

    // --- FIX #2: Options builder ---
    string options = "";
    if (model.Upsert || !string.IsNullOrWhiteSpace(model.WriteConcern))
    {
        List<string> opts = new();

        if (model.Upsert)
            opts.Add("upsert: true");

        if (!string.IsNullOrWhiteSpace(model.WriteConcern))
            opts.Add($"writeConcern: {{ w: \"{model.WriteConcern}\" }}");

        options = "{ " + string.Join(", ", opts) + " }";
    }

    // --- FIX #3: Generate final command ---
    string method = model.IsMultiple ? "updateMany" : "updateOne";

    command += 
        $"db.getCollection(\"{model.CollectionName}\")" +
        $".{method}({model.FilterJson}, {updateJson}" +
        $"{(string.IsNullOrWhiteSpace(options) ? "" : ", " + options)});";

    return command;
}

    public string GenerateDeleteScript(MongoDocumentDeleteViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.DatabaseName) ||
            string.IsNullOrWhiteSpace(model.CollectionName) ||
            string.IsNullOrWhiteSpace(model.FilterJson))
        {
            return "// Missing Database, Collection or Filter JSON";
        }

        string command = $"use {model.DatabaseName}\n";

        string options = "";
        if (!string.IsNullOrWhiteSpace(model.WriteConcern))
        {
            options = $", {{ writeConcern: {{ w: \"{model.WriteConcern}\" }} }}";
        }

        if (model.IsMultiple)
            command += $"db.getCollection(\"{model.CollectionName}\").deleteMany({model.FilterJson}{options});";
        else
            command += $"db.getCollection(\"{model.CollectionName}\").deleteOne({model.FilterJson}{options});";

        return command;
    }

}
public class MongoQueryService : IMongoQueryService
{
    public string GenerateQueryScript(MongoQueryViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.DatabaseName) ||
            string.IsNullOrWhiteSpace(model.CollectionName))
        {
            return "// Database or Collection is missing.";
        }

        string script = $"use {model.DatabaseName}\n";

        string filter = string.IsNullOrWhiteSpace(model.FilterJson) ? "{}" : model.FilterJson;
        string projection = string.IsNullOrWhiteSpace(model.ProjectionJson) ? "{}" : model.ProjectionJson;
        string sort = string.IsNullOrWhiteSpace(model.SortJson) ? "{}" : model.SortJson;

        script += $"db.getCollection(\"{model.CollectionName}\")\n" +
                  $"    .find({filter}, {projection})\n" +
                  $"    .sort({sort})\n";

        if (model.Skip.HasValue)
            script += $"    .skip({model.Skip.Value})\n";

        if (model.Limit.HasValue)
            script += $"    .limit({model.Limit.Value})\n";

        script += $"    ;";

        return script;
    }
}


public class MongoBulkService: IMongoBulkService
{
    public string GenerateBulkScript(MongoBulkViewModel model)
    {
        string db = model.DatabaseName ?? "TestDB";
        string col = model.CollectionName ?? "TestCollection";

        string script = $"use {db};\n\n";

        switch (model.OperationType)
        {
            case "insertMany":
                script += $"db.getCollection(\"{col}\").insertMany([\n";
                script += string.Join(",\n", model.InsertDocuments);
                script += "\n]);";
                break;

            case "updateMany":
                script +=
                    $"db.getCollection(\"{col}\")\n" +
                    $".updateMany(\n" +
                    $"{(string.IsNullOrWhiteSpace(model.UpdateFilterJson) ? "{}" : model.UpdateFilterJson)},\n" +
                    $"{(string.IsNullOrWhiteSpace(model.UpdateJson) ? "{}" : model.UpdateJson)}\n" +
                    ");";
                break;

            case "deleteMany":
                script +=
                    $"db.getCollection(\"{col}\")\n" +
                    $".deleteMany({(string.IsNullOrWhiteSpace(model.DeleteFilterJson) ? "{}" : model.DeleteFilterJson)});";
                break;
        }

        return script;
    }
}
public class MongoViewService: IMongoViewService
{
    public string GenerateScript(MongoViewViewModel model)
    {
        string db = model.DatabaseName ?? "TestDB";
        string view = model.ViewName ?? "MyView";
        string collection = model.SourceCollection ?? "Users";

        string pipeline = model.PipelineJson;
        if (string.IsNullOrWhiteSpace(pipeline))
            pipeline = "[]";

        string script = $"use {db};\n\n";

        switch (model.OperationType)
        {
            case "create":
                script +=
$@"db.createView(
    ""{view}"",
    ""{collection}"",
    {pipeline}
);";
                break;

            case "replace":
                script +=
$@"db.runCommand({{
  collMod: ""{view}"",
  viewOn: ""{collection}"",
  pipeline: {pipeline}
}});";
                break;

            case "delete":
                script += $@"db.{view}.drop();";
                break;
        }

        return script;
    }
}
public class RedisDatabaseService: IRedisDatabaseService
{
    public string GenerateSelectDatabase(int db)
    {
        return $"SELECT {db}";
    }

    public string GenerateClearDatabase(int db)
    {
        return
$@"SELECT {db}
FLUSHDB  # Clears only this database";
    }

    public string GenerateExport(int db, string format)
    {
        string exportCommand = format switch
        {
            "json" => "# Redis does not export JSON natively — use redis-tools\nredis-dump --db {db}",
            "rdb" => "SAVE  # Export to dump.rdb",
            "aof" => "BGREWRITEAOF  # Export AOF",
            _ => "# Invalid format"
        };

        return
$@"SELECT {db}
{exportCommand}";
    }
}

public class RedisScriptService : IRedisScriptService
{
    public string GenerateClearDatabaseScript(int db)
    {
        return $"SELECT {db}\nFLUSHDB";
    }


    public string GenerateClearAllDatabaseScript()
    {
        return $"FLUSHALL";
    }

    public string GeneratePatternDeleteScript(int db, string pattern)
    {
        return
    $@"SELECT {db}
EVAL ""for _,k in ipairs(redis.call('keys','{pattern}')) do redis.call('del',k) end"" 0";
    }
    public string GenerateDeleteExpiredKeysScript(int db)
    {
        return
    $@"SELECT {db}

EVAL [[
local cursor = 0
repeat
  local result = redis.call('SCAN', cursor)
  cursor = tonumber(result[1])
  local keys = result[2]

  for _, key in ipairs(keys) do
    if redis.call('TTL', key) ~= -1 then
      if redis.call('TTL', key) < 0 then
        redis.call('DEL', key)
      end
    end
  end

until cursor == 0
return true
]] 0";
    }

}

public class RedisKeyScriptService : IRedisKeyScriptService
{
    public string AddKey(int db, string key, string value, int? ttl)
    {
        var ttlPart = ttl.HasValue ? $"\nEXPIRE {key} {ttl}" : "";
        return $"SELECT {db}\nSET {key} \"{value}\"{ttlPart}";
    }

    public string UpdateKey(int db, string key, string newValue)
    {
        return $"SELECT {db}\nSET {key} \"{newValue}\"";
    }

    public string DeleteKey(int db, string key)
    {
        return $"SELECT {db}\nDEL {key}";
    }

    public string SetTTL(int db, string key, int? ttl)
    {
        if (ttl == null)
            return $"SELECT {db}\nPERSIST {key}";

        return $"SELECT {db}\nEXPIRE {key} {ttl}";
    }

    public string RenameKey(int db, string oldKey, string newKey)
    {
        return $"SELECT {db}\nRENAME {oldKey} {newKey}";
    }


    public string PersistKey(int db, string key)
    {
        return $"SELECT {db}\nPERSIST {key}";
    }

    public string CopyKey(int sourceDb, int targetDb, string key)
    {
        return
$@"SELECT {sourceDb}
DUMP {key}
SELECT {targetDb}
RESTORE {key} 0 serialized_value
";
    }

    public string MoveKey(int sourceDb, int targetDb, string key)
    {
        return
$@"SELECT {sourceDb}
MOVE {key} {targetDb}";
    }

}


public class RedisKeyInspectorService : IRedisKeyInspectorService
{
    public string InspectKey(RedisKeyInspectorViewModel model)
    {
        var commands = new List<string> { $"SELECT {model.DatabaseNumber}" };

        if (model.ShowType)
            commands.Add($"TYPE {model.Key}");

        if (model.ShowTTL)
            commands.Add($"TTL {model.Key}");

        if (model.ShowLength)
            commands.Add($"STRLEN {model.Key}  ; for string length, or LLEN/LEN for list/set/hash etc.");

        if (model.ShowMemoryUsage)
            commands.Add($"MEMORY USAGE {model.Key}");

        return string.Join("\n", commands);
    }
}

public class RedisServerCommandService : IRedisServerCommandService
{
    public string GenerateServerCommands(RedisServerInfoViewModel model)
    {
        var commands = new List<string>();

        if (model.ShowInfo)
            commands.Add("INFO");

        if (model.ShowMemoryStats)
            commands.Add("MEMORY STATS");

        if (model.ShowClientList)
            commands.Add("CLIENT LIST");

        if (model.ShowSlowLog)
            commands.Add("SLOWLOG GET");

        return string.Join("\n", commands);
    }
}

public class RedisSecurityService : IRedisSecurityService
{
    public string RequirePass(string password)
    {
        return $"CONFIG SET requirepass {password}";
    }

    public string ACLSetUser(string username, string password, List<string> permissions)
    {
        var perms = permissions != null && permissions.Any()
            ? string.Join(" ", permissions)
            : "~* +@all";
        return $"ACL SETUSER {username} on >{password} {perms}";
    }

    public string ACLDelUser(string username)
    {
        return $"ACL DELUSER {username}";
    }

    public string ACLList()
    {
        return "ACL LIST";
    }

    public string ResetPass(string username, string newPassword)
    {
        return $"ACL SETUSER {username} >{newPassword}";
    }
}

public class RedisKeyService : IRedisKeyService
{
    public string Set(string key, string value, int? expireSeconds = null)
    {
        return expireSeconds.HasValue
            ? $"SET {key} \"{value}\" EX {expireSeconds}"
            : $"SET {key} \"{value}\"";
    }

    public string Get(string key) => $"GET {key}";
    public string Del(string key) => $"DEL {key}";
    public string Exists(string key) => $"EXISTS {key}";
    public string Rename(string key, string newKey) => $"RENAME {key} {newKey}";
    public string TTL(string key) => $"TTL {key}";
    public string Incr(string key) => $"INCR {key}";
    public string Append(string key, string value) => $"APPEND {key} \"{value}\"";
}

public class RedisListService : IRedisListService
{
    public string LPush(string key, string value) => $"LPUSH {key} \"{value}\"";
    public string RPush(string key, string value) => $"RPUSH {key} \"{value}\"";
    public string LPop(string key) => $"LPOP {key}";
    public string LRange(string key, int start, int stop) => $"LRANGE {key} {start} {stop}";
    public string LSet(string key, int index, string value) => $"LSET {key} {index} \"{value}\"";
}

public class RedisSetService : IRedisSetService
{
    public string SAdd(string key, string value) => $"SADD {key} \"{value}\"";
    public string SRem(string key, string value) => $"SREM {key} \"{value}\"";
    public string SMembers(string key) => $"SMEMBERS {key}";
    public string SCard(string key) => $"SCARD {key}";
    public string ZAdd(string key, double score, string value) => $"ZADD {key} {score} \"{value}\"";
    public string ZRem(string key, string value) => $"ZREM {key} \"{value}\"";
    public string ZRange(string key, int start, int stop) => $"ZRANGE {key} {start} {stop}";
    public string ZRangeByScore(string key, double minScore, double maxScore) => $"ZRANGEBYSCORE {key} {minScore} {maxScore}";
}
public class RedisHashService : IRedisHashService
{
    public string HSet(string key, string field, string value) => $"HSET {key} {field} \"{value}\"";
    public string HGet(string key, string field) => $"HGET {key} {field}";
    public string HGetAll(string key) => $"HGETALL {key}";
    public string HDel(string key, string field) => $"HDEL {key} {field}";
}

public class RedisCounterService : IRedisCounterService
{
    public string Incr(string key, long amount = 1)
        => amount == 1 ? $"INCR {key}" : $"INCRBY {key} {amount}";

    public string Decr(string key, long amount = 1)
        => amount == 1 ? $"DECR {key}" : $"DECRBY {key} {amount}";
}




public class RedisSearchService : IRedisSearchService
{
    public string Scan(string pattern, int? count)
    {
        string countPart = count.HasValue ? $"COUNT {count}" : "";
        return $"SCAN 0 MATCH {pattern} {countPart}".Trim();
    }

    public string FilterMatch(string pattern)
        => $"SCAN 0 MATCH {pattern}";

    public string FilterBuilder(string pattern, string filter)
        => $"SCAN 0 MATCH {pattern} | FILTER {filter}";
}
