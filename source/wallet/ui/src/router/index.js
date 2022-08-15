import { createRouter, createWebHistory } from "vue-router";

import store from "../store";
import Home from "../views/HomeView.vue";
import Login from "../views/LoginView.vue";
import Send from "../views/SendView.vue";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/",
      name: "home",
      component: Home,
      meta: { requiresLogin: true },
    },
    {
      path: "/login",
      name: "login",
      component: Login,
    },
    {
      path: "/send",
      name: "send",
      component: Send,
    }
  ],
});

router.beforeEach((to, from, next) => {
  if (store.state.wallet.instance && to.path === "/login") {
    return next({ name: "home" });
  }

  if (to.meta && to.meta.requiresLogin && !store.state.wallet.instance) {
    return next({ name: "login" });
  }

  return next();
});

export default router;
