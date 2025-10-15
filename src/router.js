import { createRouter, createWebHistory } from 'vue-router';
import MainPage from './views/MainPage.vue';
import AccountPage from './views/Account.vue';

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
        path: '/account',
        name: 'Account',
        component: AccountPage
    }
]

const router = createRouter({
    history: createWebHistory(),
    routes
});
export default router;