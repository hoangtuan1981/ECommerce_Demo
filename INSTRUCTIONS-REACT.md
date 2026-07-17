# React Instructions

## File Locations
```
frontend/src/features/
└── feature-name/
    ├── hooks/
    ├── pages/
    ├── schemas/
    ├── store/
    ├── components/
    ├── services/
    ├── types/
    └── index.ts
```

## FmgDataGrid (MUI X Data Grid Pro)
All contact pages use server-side modes:
- `paginationMode="server"`
- `sortingMode="server"`
- `filterMode="server"`

### Page-Specific Settings

| Page | pageSizeOptions | Default Sort | Select All |
|------|-----------------|--------------|------------|
| ManageContacts | `[20, 50, 100]` | `fullName` | Yes |
| ManageGroups | `[20, 50, 100, 200]` | `name` | Yes |
| GroupDetail | `[5, 10, 20, 50]` | `fullName` | Yes |

## State Management Pattern
```tsx
const [data, setData] = useState<T[]>([]);
const [totalCount, setTotalCount] = useState(0);
const [isGridLoading, setIsGridLoading] = useState(false);
const [paginationModel, setPaginationModel] = useState({ page: 0, pageSize: 20 });
const [sortModel, setSortModel] = useState<GridSortModel>([{ field: "defaultField", sort: "asc" }]);
const [searchInput, setSearchInput] = useState("");
const [searchQuery, setSearchQuery] = useState("");
```

## Behaviors
- **Pagination:** Page resets to 0 when `pageSize` or `searchQuery` changes
- **Search:** Debounced until Search button or Enter key; `searchInput` ≠ `searchQuery`

## Select All Across Pages
`UseSelectedAllServerDataGrid<T>` hook provides:
- `rowSelectionModel` — For FmgDataGrid checkbox selection
- `selectedRows` / `unselectedRows` — Arrays for business logic
- `isSelectAll` — Flag when "select all across pages" is active
- `selectionContextValue` — For `<SelectionContext.Provider>`

## UI Conventions
- Black header banner with white title and badge count
- Settings dropdown (gear icon) in header for bulk actions
- Action buttons (Add, Delete) in toolbar below header
- Search on right side of toolbar

---

## Unit Tests

### Contact Page Tests
Run all contact page tests:
```bash
npm test -- --testPathPattern="(ManageContacts.test.tsx|ManageGroups.test.tsx|GroupDetail.test.tsx)"
```

### Individual Test Files
| Component | Test File | Command |
|-----------|-----------|---------|
| ManageContacts | `tests/areas/contacts/ManageContacts/ManageContacts.test.tsx` | `npm test -- --testPathPattern="ManageContacts.test.tsx"` |
| ManageGroups | `tests/areas/contacts/ManageGroups/ManageGroups.test.tsx` | `npm test -- --testPathPattern="ManageGroups.test.tsx"` |
| GroupDetail | `tests/areas/contacts/ManageGroups/GroupDetail.test.tsx` | `npm test -- --testPathPattern="GroupDetail.test.tsx"` |

### All Contact Area Tests
```bash
npm test -- --testPathPattern="areas/contacts"
```

---

## ESLint Rules

### No nested ternary expressions (`no-nested-ternary`)
The pre-commit hook runs ESLint on staged files. Nested ternary expressions are **not allowed**.

**Bad:**
```tsx
{loading ? (
    <Spinner />
) : items.length === 0 ? (
    <Empty />
) : (
    <List />
)}
```

**Good — use separate conditional renders instead:**
```tsx
{loading && <Spinner />}
{!loading && items.length === 0 && <Empty />}
{!loading && items.length > 0 && <List />}
```

---

## Known Issues

### Header checkbox "Select All" only selects 1 row on page 2+
- **Root Cause:** MUI's `gridPaginatedVisibleSortedGridRowIdsSelector` returns incorrect row IDs for server-side pagination (virtual scrolling bug)
- **Solution:** Pass `visibleRowIds` through `SelectionContext` from the `rows` prop instead of MUI selector
- **Reference:** `shared/checkboxSelectionColumn.tsx`, `UseSelectedAllServerDataGrid.tsx`

### Custom checkbox column conflicts with `checkboxSelection` prop
- **Root Cause:** Both use field `"__check__"` with competing event handlers
- **Solution:** Use field `"selection"`, remove `type: "checkboxSelection"`, remove `checkboxSelection` prop, add `keepNonExistentRowsSelected`
- **Reference:** `shared/checkboxSelectionColumn.tsx`, wrap grid with `<SelectionContext.Provider>`
