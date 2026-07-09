# Database Instructions

## Schema Context
Use these table definitions to ensure accurate property names and relationships when writing queries.

## Tables

### [nwsltr].[Contact]
```sql

```

### [nwsltr].[Address]
```sql

```

### [nwsltr].[Group]
```sql

```

### [nwsltr].[GroupContact]
```sql

```
### [dbo].[CRMRemoteContactMapping]
```sql

```

## Table Relationships
- `Address.ContactID` → `Contact.ContactID` (FK)
- `GroupContact.ContactID` → `Contact.ContactID` (logical FK)
- `GroupContact.GroupID` → `Group.GroupID` (logical FK)
