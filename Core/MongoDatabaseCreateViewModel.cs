using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core;
public class MongoListDatabasesViewModel
{
    public string? GeneratedScript { get; set; }
}

public class MongoDatabaseCreateViewModel
{
    public string DatabaseName { get; set; }

    // Collation
    public bool? UseCollation { get; set; }
    public string? CollationLocale { get; set; }
    public int? CollationStrength { get; set; }
    public bool? NumericOrdering { get; set; }

    // User
    public bool? CreateUser { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? UserRole { get; set; }

    // Sharding
    public bool? EnableSharding { get; set; }

    // Write Concern
    public bool? UseWriteConcern { get; set; }
    public string? W { get; set; }
    public bool? J { get; set; }
    public int? WTimeout { get; set; }

    // Read Concern
    public bool? UseReadConcern { get; set; }
    public string? ReadConcern { get; set; }

    // Default Collection
    public bool? CreateDefaultCollection { get; set; }
    public string? CollectionName { get; set; }

    public string? GeneratedScript { get; set; }
}
public class MongoDropDatabaseViewModel
{
    [Required]
    [Display(Name = "Database Name")]
    public string DatabaseName { get; set; }

    [Display(Name = "Force Drop (if exists)")]
    public bool ForceDelete { get; set; }

    public string? GeneratedScript { get; set; }
}
public class MongoUseDatabaseViewModel
{
    [Required]
    [Display(Name = "Database Name")]
    public string DatabaseName { get; set; }

    public string? GeneratedScript { get; set; }
}


public class MongoCreateCollectionViewModel
{

    
            [Required]
    [Display(Name = "DatabaseName")]
    public string DatabaseName { get; set; }

    [Required]
    [Display(Name = "Collection Name")]
    public string CollectionName { get; set; }

    // Capped
    public bool IsCapped { get; set; }
    [Display(Name = "Max Size (bytes)")]
    public int? CappedSize { get; set; }
    [Display(Name = "Max Documents")]
    public int? CappedMax { get; set; }

    // Collation
    public bool UseCollation { get; set; }
    public string? CollationLocale { get; set; }
    public int? CollationStrength { get; set; }

    // Validation
    public bool UseValidation { get; set; }
    public string? ValidationRuleJson { get; set; }
    public string? ValidationLevel { get; set; } // off/strict/moderate

    // Comment
    public string? Comment { get; set; }

    // Write Concern
    public bool UseWriteConcern { get; set; }
    public int? WriteConcernW { get; set; }
    public bool? WriteConcernJ { get; set; }

    // TimeSeries
    public bool UseTimeSeries { get; set; }
    public string? TimeSeriesTimeField { get; set; }
    public string? TimeSeriesMetaField { get; set; }

    // Script
    public string? GeneratedScript { get; set; }
}
public class MongoDropCollectionViewModel
{


    [Required]
    [Display(Name = "DatabaseName")]
    public string DatabaseName { get; set; }

    [Required]
    [Display(Name = "Collection Name")]
    public string CollectionName { get; set; }

    public string? GeneratedScript { get; set; }
}
public class IndexField
{
    public string FieldName { get; set; }
    public string SortOrder { get; set; } // 1=Asc, -1=Desc
}

public class MongoCreateIndexViewModel
{
    [Required]
    [Display(Name = "Database Name")]
    public string DatabaseName { get; set; }

    [Required]
    [Display(Name = "Collection Name")]
    public string CollectionName { get; set; }

    [Display(Name = "Index Name (optional)")]
    public string IndexName { get; set; }

    public List<IndexField> Fields { get; set; } = new List<IndexField> { new IndexField() };

    public string IndexType { get; set; } = "Single"; // Single, Compound, Text, Hashed, 2dsphere

    // Options
    public bool Unique { get; set; }
    public bool Sparse { get; set; }
    public int? ExpireAfterSeconds { get; set; }
    public bool Background { get; set; }
    public string? PartialFilterExpression { get; set; }
    public string? CollationLocale { get; set; }
    public int? CollationStrength { get; set; }
    public string? Comment { get; set; }

    public string? GeneratedScript { get; set; }
}
public class MongoDropIndexViewModel
{
    [Required]
    [Display(Name = "Database Name")]
    public string DatabaseName { get; set; }

    [Required]
    [Display(Name = "Collection Name")]
    public string CollectionName { get; set; }

    [Required]
    [Display(Name = "Index Name")]
    public string IndexName { get; set; }

    public string? GeneratedScript { get; set; }
}
public class MongoListCollectionsViewModel
{
    [Required]
    [Display(Name = "Database Name")]
    public string DatabaseName { get; set; }

    [Display(Name = "Search Keyword")]
    public string? SearchKeyword { get; set; }

    public string? GeneratedScript { get; set; }

    // Optional: simulate results
    public string[] Collections { get; set; } = new string[0];
}
public class MongoRenameCollectionViewModel
{
    [Required]
    [Display(Name = "Database Name")]
    public string DatabaseName { get; set; }

    [Required]
    [Display(Name = "Old Collection Name")]
    public string OldCollectionName { get; set; }

    [Required]
    [Display(Name = "New Collection Name")]
    public string NewCollectionName { get; set; }

    public string? GeneratedScript { get; set; }
}
public class MongoGeneralCommandViewModel
{
    [Required]
    [Display(Name = "Command Type")]
    public string CommandType { get; set; } // e.g., ServerStatus, Ping, ListDatabases

    [Display(Name = "Database Name (Optional)")]
    public string? DatabaseName { get; set; }

    [Display(Name = "Collection Name (Optional)")]
    public string? CollectionName { get; set; }

    [Display(Name = "User Name (Optional)")]
    public string? UserName { get; set; }

    [Display(Name = "Role Name (Optional)")]
    public string? RoleName { get; set; }

    [Display(Name = "Output Script")]
    public string? GeneratedScript { get; set; }
}
public class MongoUserManagementViewModel
{
    public string ActionType { get; set; } // CreateUser, DropUser, UpdatePassword, GrantRole, RevokeRole, ListUsers

    public string UserName { get; set; }
    public string Password { get; set; }
    public string DatabaseName { get; set; }
    public List<string> SelectedRoles { get; set; } = new List<string>();

    public string? GeneratedScript { get; set; }
}

public class MongoRoleManagementViewModel
{
    public string ActionType { get; set; } // CreateRole, DropRole, AddPrivilege, RevokePrivilege, ListRoles

    public string RoleName { get; set; }
    public string? DatabaseName { get; set; }
    public string PrivilegesJson { get; set; } // Optional for add privileges
    public List<string> ParentRoles { get; set; } = new List<string>();

    public string? GeneratedScript { get; set; }
}
public class MongoAdminViewModel
{
    public MongoUserManagementViewModel UserModel { get; set; }
    public MongoRoleManagementViewModel RoleModel { get; set; }
}
public class MongoIndexViewModel
{
    public string DatabaseName { get; set; }
    public string CollectionName { get; set; }

    public List<string> Indexes { get; set; } = new List<string>(); // Script/Info for each index

    public string? GeneratedScript { get; set; }
}
public class MongoDocumentInsertViewModel
{
    public string DatabaseName { get; set; }
    public string CollectionName { get; set; }

    // JSON Document input
    public string DocumentJson { get; set; }

    // Optional: Insert many documents
    public bool IsMultiple { get; set; }

    // Optional: Write concern
    public string? WriteConcern { get; set; }

    // Output
    public string? GeneratedScript { get; set; }
}
public class MongoDocumentUpdateViewModel
{
    public string DatabaseName { get; set; }
    public string CollectionName { get; set; }

    // Filter for selecting documents
    public string FilterJson { get; set; }

    // Update document
    public string UpdateJson { get; set; }

    public bool IsMultiple { get; set; } // Update many
    public bool Upsert { get; set; }     // Insert if not exists
    public string? WriteConcern { get; set; }

    // Output
    public string? GeneratedScript { get; set; }
}

public class MongoDocumentDeleteViewModel
{
    public string DatabaseName { get; set; }
    public string CollectionName { get; set; }

    // JSON filter to match documents
    public string FilterJson { get; set; }

    // Options
    public bool IsMultiple { get; set; } // deleteMany
    public string? WriteConcern { get; set; }

    // Output script
    public string? GeneratedScript { get; set; }
}

public class MongoQueryViewModel
{
    public string DatabaseName { get; set; }
    public string CollectionName { get; set; }

    // Search filters
    public string FilterJson { get; set; }
    public string ProjectionJson { get; set; }
    public string SortJson { get; set; }

    public int? Limit { get; set; }
    public int? Skip { get; set; }

    // helper to build query from UI
    public List<QueryConditionItem> Conditions { get; set; } = new();

    public string? GeneratedScript { get; set; }
}

public class QueryConditionItem
{
    public string Field { get; set; }
    public string Operator { get; set; } // eq, gt, lt, regex...
    public string Value { get; set; }
}
public class MongoBulkViewModel
{
    public string DatabaseName { get; set; }
    public string CollectionName { get; set; }

    // Insert
    public List<string> InsertDocuments { get; set; } = new List<string>();

    // Update
    public string? UpdateFilterJson { get; set; }
    public string? UpdateJson { get; set; }

    // Delete
    public string? DeleteFilterJson { get; set; }

    public string OperationType { get; set; } // insertMany | updateMany | deleteMany

    public string? GeneratedScript { get; set; }
}
public class MongoViewViewModel
{
    public string DatabaseName { get; set; }
    public string ViewName { get; set; }
    public string SourceCollection { get; set; }

    public string PipelineJson { get; set; }
    public string OperationType { get; set; } // create | replace | delete | list

    public string? GeneratedScript { get; set; }

    // For list views
    public List<ViewInfo> Views { get; set; } = new();
}

public class ViewInfo
{
    public string Name { get; set; }
    public string Type { get; set; }
    public string Pipeline { get; set; }
}


public class AdvancedIndexViewModel
{
    public string DatabaseName { get; set; }
    public string CollectionName { get; set; }

    public List<IndexAdvancedField> Fields { get; set; } = new(); // برای compound

    public bool IsUnique { get; set; }
    public bool IsSparse { get; set; }
    public bool IsHidden { get; set; }
    public bool IsText { get; set; }
    public bool IsHashed { get; set; }
    public bool IsWildcard { get; set; }
    public bool IsTTL { get; set; }
    public int? TTLSeconds { get; set; }

    public string PartialFilter { get; set; }
    public string Collation { get; set; }
    public string CustomIndexName { get; set; }

    public string? GeneratedScript { get; set; }
}

public class IndexAdvancedField
{
    public string FieldName { get; set; }
    public string Order { get; set; } // 1, -1, text, hashed
}
public class RedisDatabaseViewModel
{
    public int DatabaseNumber { get; set; } = 0;
    public bool ConfirmDangerous { get; set; }
    public string ExportFormat { get; set; } // json | rdb | aof

    public string? GeneratedScript { get; set; }
}
public class RedisClearDatabaseViewModel
{
    public int DatabaseNumber { get; set; } = 0;

  
    public string? GeneratedScript { get; set; }
}
public class RedisClearAllDatabaseViewModel
{



    public string? GeneratedScript { get; set; }
}
public class RedisClearPatternViewModel
{
    public int DatabaseNumber { get; set; }
    public string Pattern { get; set; }
    public string? GeneratedScript { get; set; }
}
public class RedisClearExpiredViewModel
{
    public int DatabaseNumber { get; set; }
    public string? GeneratedScript { get; set; }
}
public class RedisKeyAddViewModel
{
    public int DatabaseNumber { get; set; } = 0;
    public string Key { get; set; }
    public string Value { get; set; }
    public int? TTLSeconds { get; set; }
    public string? GeneratedScript { get; set; }
}

public class RedisKeyUpdateViewModel
{
    public int DatabaseNumber { get; set; } = 0;
    public string Key { get; set; }
    public string NewValue { get; set; }
    public string? GeneratedScript { get; set; }
}

public class RedisKeyDeleteViewModel
{
    public int DatabaseNumber { get; set; } = 0;
    public string Key { get; set; }
    public string? GeneratedScript { get; set; }
}

public class RedisKeyTTLViewModel
{
    public int DatabaseNumber { get; set; } = 0;
    public string Key { get; set; }
    public int? TTLSeconds { get; set; }
    public string? GeneratedScript { get; set; }
}

public class RedisKeyRenameViewModel
{
    public int DatabaseNumber { get; set; } = 0;
    public string OldKey { get; set; }
    public string NewKey { get; set; }
    public string? GeneratedScript { get; set; }
}

public class RedisKeyPersistViewModel
{
    public int DatabaseNumber { get; set; } = 0;
    public string Key { get; set; }
    public string? GeneratedScript { get; set; }
}

public class RedisKeyCopyViewModel
{
    public int SourceDatabase { get; set; } = 0;
    public int TargetDatabase { get; set; } = 0;
    public string Key { get; set; }
    public string? GeneratedScript { get; set; }
}

public class RedisKeyMoveViewModel
{
    public int SourceDatabase { get; set; } = 0;
    public int TargetDatabase { get; set; } = 0;
    public string Key { get; set; }
    public string? GeneratedScript { get; set; }
}
public class RedisKeyInspectorViewModel
{
    public int DatabaseNumber { get; set; } = 0;
    public string Key { get; set; }

    public bool ShowType { get; set; } = true;
    public bool ShowTTL { get; set; } = true;
    public bool ShowLength { get; set; } = true;
    public bool ShowMemoryUsage { get; set; } = true;

    public string? GeneratedScript { get; set; }
}
public class RedisServerInfoViewModel
{
    public bool ShowInfo { get; set; } = true;
    public bool ShowMemoryStats { get; set; } = true;
    public bool ShowClientList { get; set; } = true;
    public bool ShowSlowLog { get; set; } = true;

    public string? GeneratedScript { get; set; }
}
public class RedisRequirePassViewModel
{
    public string Password { get; set; }
    public string? GeneratedScript { get; set; }
}

public class RedisACLUserViewModel
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public List<string> Permissions { get; set; } = new List<string>();
    public string? GeneratedScript { get; set; }
}
public class RedisACLDelUserViewModel
{
    public string UserName { get; set; }
    public List<string> Permissions { get; set; } = new List<string>();
    public string? GeneratedScript { get; set; }
}
public class RedisACLListViewModel
{
    public string? GeneratedScript { get; set; }
}

public class RedisResetPassViewModel
{
    public string UserName { get; set; }
    public string NewPassword { get; set; }
    public string? GeneratedScript { get; set; }
}
public class RedisKeyOperationViewModel
{
    public string Key { get; set; }
    public string Value { get; set; }
    public int? ExpireSeconds { get; set; }
    public string NewKey { get; set; } // For RENAME
    public string? GeneratedScript { get; set; }
}
public class RedisListOperationViewModel
{
    public string Key { get; set; }
    public string Value { get; set; }
    public int? Index { get; set; } // For LSET
    public int? Start { get; set; } // For LRANGE
    public int? Stop { get; set; }  // For LRANGE
    public string? GeneratedScript { get; set; }
}
public class RedisSetOperationViewModel
{
    public string Key { get; set; }
    public string Value { get; set; }
    public double? Score { get; set; } // For Sorted Set
    public int? Start { get; set; } // For ZRANGE
    public int? Stop { get; set; }  // For ZRANGE
    public double? MinScore { get; set; } // For ZRANGEBYSCORE
    public double? MaxScore { get; set; } // For ZRANGEBYSCORE
    public string? GeneratedScript { get; set; }
}
public class RedisHashOperationViewModel
{
    public string Key { get; set; }
    public string Field { get; set; }
    public string Value { get; set; }
    public string? GeneratedScript { get; set; }
}
public class RedisCounterViewModel
{
    public string Key { get; set; }
    public long? Amount { get; set; } = 1; // Default increment/decrement
    public string? GeneratedScript { get; set; }
}
public class RedisSearchViewModel
{
    public string Pattern { get; set; } = "*"; // Default: all keys
    public int? Count { get; set; } = 10;      // Number of keys per SCAN
    public string? Filter { get; set; }         // Optional filter expression
    public string? GeneratedScript { get; set; }
}
