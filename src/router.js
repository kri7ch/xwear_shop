import { createRouter, createWebHistory } from 'vue-router';
import MainPage from './views/MainPage.vue';
import AccountPage from './views/Account.vue';
import CatalogPage from './views/CatalogPage.vue';
import ProductPage from './views/ProductPage.vue';
import CartPage from './views/CartPage.vue';

const routes = [
    {
        path: '/',
        redirect: '/mainpage'
    },
    {
        path: '/mainpage',
        name: 'MainPage',
        component: MainPage
    },
    {
        path: '/catalog',
        name: 'Catalog',
        component: CatalogPage
    },
    {
        path: '/product/:id',
        name: 'Product',
        component: ProductPage
    },
    {
        path: '/account',
        name: 'Account',
        component: AccountPage,
        meta: { requiresAuth: true }
    },
    {
        path: '/cart',
        name: 'Cart',
        component: CartPage
    },
    {
        path: '/checkout',
        name: 'Checkout',
        component: () => import('./views/CheckoutPage.vue'),
        meta: { requiresAuth: true }
    }
]

const router = createRouter({
    history: createWebHistory(),
    routes
});

router.beforeEach(async (to, from, next) => {
    if (to.name === 'Account' && to.query.mode === 'login') {
        return next();
    }
    if (!to.meta.requiresAuth) {
        return next();
    }
    try {
        const response = await fetch('/api/users/profile', { credentials: 'include' })
      if (response.ok) {
            return next();
        }
        return next({ name: 'Account', query: { mode: 'login' } });
    } catch (e) {
        return next({ name: 'Account', query: { mode: 'login' } });
    }
});

export default router;
