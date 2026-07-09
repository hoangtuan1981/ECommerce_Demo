# Implementation Instructions

## Instruction Files Tree
Read files based on what you're working on:

```
D:\FMG Source Code\markdown-files\
├── INSTRUCTIONS.md              ← This file (always read first)
├── INSTRUCTIONS-BACKEND.md      ← .NET API: CQRS, MediatR, V5 patterns, naming
├── INSTRUCTIONS-DATABASE.md     ← SQL schema: Contact, Address, Group tables
├── INSTRUCTIONS-REACT.md        ← FMG.React: grid, hooks, known issues
└── {branch-part}/               ← Feature branch folders
    ├── {feature}-plan.md        ← Implementation plan
    └── {feature}-endpoints.md   ← Endpoint documentation
```

## Before Starting Any Implementation

1. **Read the relevant instruction file(s)** based on your task:
   - Backend/API work → `INSTRUCTIONS-BACKEND.md`
   - Database queries → `INSTRUCTIONS-DATABASE.md`
   - React/Frontend → `INSTRUCTIONS-REACT.md`

2. **Check for branch-specific plans:**
   - Look for a folder matching part of the current git branch name
   - Read all `.md` files inside for context, plan, and feedback

3. **Review `///feedback:` comments**
   - Inline feedback format: `///feedback: <message>`
   - Apply corrections before implementation
   - Remove the `///feedback:` lines after applying

---

## Code Principles

1. **Think before code** — If unsure about anything, ask to confirm. Never guess.
2. **Simplicity first** — Prefer the simplest solution that meets the requirement.
3. **Surgical edits** — Edit only the correct place. Do not clean up or refactor out-of-scope code.
4. **Goal driven** — Stay focused on the task objective. Every change must serve the goal.

---

## Your Role
- You are a .NET and React expert
- Ensure accuracy, consistency, security best practices, follow SOLID principles
- Create code with good performance; flag any performance concerns

---

## Workflow

1. **Plan** — Create/update markdown plan in branch-specific folder
2. **Review** — User adds `///feedback:` comments
3. **Apply feedback** — Update plan based on feedback
4. **Implement** — Follow instruction files, plan, and **Handler Design Rules** below
5. **Validate** — Run tests and check for compile errors
6. **Verify flowchart** — As the final step, trace the call flow of the feature from entry point (controller/handler) through all method calls. Build a flowchart and check:
   - No method is called/invoked more than once per request execution (excluding intentional loops)
   - DB writes and side effects are orchestrated from the handler, not buried in downstream services
   - Each service method has a single responsibility (fetch/compute only, no mixed DB writes)
   - If duplicates are found, refactor to consolidate into a single call site

## Handler Design Rules (Apply During Implementation)

When writing a Feature Handler, apply these rules **from the start** — don't write first and refactor later.

### 1. Define the step flow first
Before writing code, list the numbered steps the handler will execute. This becomes the handler's structure.

### 2. Handler = orchestration only
The handler decides _what_ to do and _in what order_. It should read as a linear sequence of high-level steps — no low-level logic.

### 3. Identify reusable sequences early
If two or more steps will follow the same pattern (e.g., getFields → search → fetchGroups → markSynced for different object types), create **one service method** that encapsulates the full sequence and call it for each variant.

### 4. Shared data fetched once, passed down
Data needed by multiple steps (e.g., `syncedCampaignIds`, feature flags) must be loaded early and passed as parameters to service methods — never re-fetched inside a service.

### 5. Each block is self-contained
A service method should complete its full unit of work (search + enrich + mark) so the handler doesn't need to stitch together partial results across steps.

See `INSTRUCTIONS-BACKEND.md` → **Handler Refactoring Steps** for the detailed extraction pattern when refactoring existing code.
