import { createRouter, createWebHistory } from 'vue-router';
import MainPage from './components/MainPage.vue';

const routes = [
    {
        path: '/',
        redirect: '/mainpage'
    },
    {
        path: '/mainpage',
        name: 'MainPage',
        component: MainPage
    }
]

const router = createRouter({
    history: createWebHistory(),
    routes
});
export default router;