using Core;
using Service.Services;

namespace Service.Interfaces;

public interface IMongoScriptService
{
    string GenerateCreateDatabaseScript(MongoDatabaseCreateViewModel model);
    string GenerateDropDatabaseScript(string dbName, bool forceDelete);

    string GenerateListDatabasesScript();
    string GenerateUseDatabaseScript(string dbName);

}
public interface IMongoCollectionScriptService
{

    string GenerateCreateCollectionScript(MongoCreateCollectionViewModel m);
    string GenerateDropCollectionScript(MongoDropCollectionViewModel m);

    string GenerateShowCollectionsScript(MongoListCollectionsViewModel m);

    string GenerateRenameCollectionScript(MongoRenameCollectionViewModel m);
}
public interface IMongoIndexScriptService
{

    string GenerateCreateIndexScript(MongoCreateIndexViewModel m);
    string GenerateDropIndexScript(MongoDropIndexViewModel m);


    // Generates a MongoDB command to list indexes of a collection
    string GenerateListIndexesScript(string database, string collection);

    // Optional: format Index info (mocked for UI)
    List<string> MockIndexes(string collection);

    string AdvanceIndexGenerateScript(AdvancedIndexViewModel vm);
}

public interface IMongoInfoService
{
    string GenerateGeneralCommandScript(MongoGeneralCommandViewModel m);


}
public interface IMongoSecurityService
{
    string GenerateUserScript(MongoUserManagementViewModel m);

    string GenerateRoleScript(MongoRoleManagementViewModel m);

}
public interface IMongoDocumentService
{
    string GenerateInsertScript(MongoDocumentInsertViewModel model);
    string GenerateUpdateScript(MongoDocumentUpdateViewModel model);

    string GenerateDeleteScript(MongoDocumentDeleteViewModel model);
}

public interface IMongoQueryService
{
    string GenerateQueryScript(MongoQueryViewModel model);

}
public interface IMongoBulkService
{
    string GenerateBulkScript(MongoBulkViewModel model);
}


public interface IMongoViewService
{
    string GenerateScript(MongoViewViewModel model);
}
public interface IRedisDatabaseService
{
    string GenerateSelectDatabase(int db);

    string GenerateClearDatabase(int db);
    string GenerateExport(int db, string format);

}
public interface IRedisScriptService
{
    string GenerateClearDatabaseScript(int db);

    string GenerateClearAllDatabaseScript();
     string GeneratePatternDeleteScript(int db, string pattern);


     string GenerateDeleteExpiredKeysScript(int db);
}
public interface IRedisKeyScriptService
{
    string AddKey(int db, string key, string value, int? ttl);
    string UpdateKey(int db, string key, string newValue);
    string DeleteKey(int db, string key);
    string SetTTL(int db, string key, int? ttl);
    string RenameKey(int db, string oldKey, string newKey);

    string PersistKey(int db, string key);
    string CopyKey(int sourceDb, int targetDb, string key);
    string MoveKey(int sourceDb, int targetDb, string key);

}

public interface IRedisKeyInspectorService
{
    string InspectKey(RedisKeyInspectorViewModel model);
}
public interface IRedisServerCommandService
{
    string GenerateServerCommands(RedisServerInfoViewModel model);
}
public interface IRedisSecurityService
{
    string RequirePass(string password);
    string ACLSetUser(string username, string password, List<string> permissions);
    string ACLDelUser(string username);
    string ACLList();
    string ResetPass(string username, string newPassword);
}
public interface IRedisKeyService
{
    string Set(string key, string value, int? expireSeconds = null);
    string Get(string key);
    string Del(string key);
    string Exists(string key);
    string Rename(string key, string newKey);
    string TTL(string key);
    string Incr(string key);
    string Append(string key, string value);
}
public interface IRedisListService
{
    string LPush(string key, string value);
    string RPush(string key, string value);
    string LPop(string key);
    string LRange(string key, int start, int stop);
    string LSet(string key, int index, string value);
}
public interface IRedisSetService
{
    string SAdd(string key, string value);
    string SRem(string key, string value);
    string SMembers(string key);
    string SCard(string key);
    string ZAdd(string key, double score, string value);
    string ZRem(string key, string value);
    string ZRange(string key, int start, int stop);
    string ZRangeByScore(string key, double minScore, double maxScore);
}


public interface IRedisHashService
{
    string HSet(string key, string field, string value);
    string HGet(string key, string field);
    string HGetAll(string key);
    string HDel(string key, string field);
}
public interface IRedisCounterService
{
    string Incr(string key, long amount = 1);
    string Decr(string key, long amount = 1);
}

public interface IRedisSearchService
{
    string Scan(string pattern, int? count);
    string FilterMatch(string pattern);
    string FilterBuilder(string pattern, string filter);
}
