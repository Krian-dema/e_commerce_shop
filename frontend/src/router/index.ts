import { createRouter, createWebHistory } from 'vue-router';

// import.meta.env.BASE_URL
const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: '/',
      component: () => import('@/views/Dashboard.vue'),
    },
  ],
  scrollBehavior() {
    return { top: 0 };
  },
});

export default router;
