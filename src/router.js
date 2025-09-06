import { createRouter, createWebHistory } from 'vue-router';
import MainPage from './components/MainPage.vue';

const routes = [
    {
        path: '/mainpage',
        name: 'MainPage',
        component: MainPage
    }
]

const router = createRouter({
    history: createWebHistory(),
    routes,
    scrollBehavior(to, from, savedPosition) {
        if (to.hash) {
        return {
            el: to.hash,
            behavior: 'smooth',
            top: 100
        }
        }
        return savedPosition || { top: 0, behavior: 'smooth' }
    }
});
export default router;