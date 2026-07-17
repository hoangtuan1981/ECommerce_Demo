# TypeScript
    https://www.typescriptlang.org/
    

# grok

 F:\Investigate\eCommerce\src\frontend> 
    npm install
    npm run build
    npm run dev

frontend/
├── public/
├── src/
│   ├── api/                  # Axios instances + interceptors
│   ├── assets/
│   ├── components/           # UI components (shadcn/ui)
│   ├── features/             # Feature-based (auth, cart, products...)
│   │   └── auth/
│   ├── hooks/                # Custom hooks
│   ├── layouts/
│   ├── lib/                  # Utils, validators (Zod)
│   ├── pages/                # Routes
│   ├── store/                # Zustand stores
│   ├── types/                # TypeScript definitions
│   ├── App.tsx
│   ├── main.tsx
│   └── routes.tsx
├── .env
├── .env.example
├── vite.config.ts
├── tailwind.config.ts
├── tsconfig.json
├── package.json
└── dockerfile



# Giải thích axios
    Sử dụng trong hook
        import { api } from '@/api';
    Thay vì:
        import api from '@/api/client';

src/
├── api/
│ ├── client.ts
│ ├── endpoints.ts
│ ├── interceptors.ts
│ ├── auth.ts
│ └── index.ts
├── features/
│ ├── auth/
│ ├── product/
│ ├── cart/
│ └── order/