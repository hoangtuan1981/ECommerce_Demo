"TODO: product, cart, order"

export const ENDPOINTS = {
  identity: {
    login: '/api/identity/auth/login',
    refresh: '/api/identity/auth/refresh',
    register: '/api/identity/auth/register',
    logout: '/api/identity/auth/logout',
  },

  product: {
    list: '/api/product/products',
    detail: (id: string) => `/api/product/products/${id}`,
  },

  cart: {
    items: '/api/cart/items',
  },

  order: {
    orders: '/api/order/orders',
  },
};