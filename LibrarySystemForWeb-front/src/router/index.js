import Vue from 'vue'
import Router from 'vue-router'
import { Message } from 'element-ui'

Vue.use(Router)

var router = new Router({
  routes: [
    {
      path: '/',
      name: 'login',
      component: () => import('../view/Login.vue')
    },
    {
      path: '/main',
      name: '',
      component: () => import('../view/Layout.vue'),
      meta: {
        requiresAuth: true
      },
      children: [
        {
          path: '/',
          name: 'welcome',
          component: () => import('../view/Welcome.vue'),
          meta: {
            requiresAuth: true
          },
        },
        {
          path: '/user',
          name: 'user',
          component: () => import('../view/User.vue'),
          meta: {
            requiresAuth: true
          },
        },
        {
          path: '/reader',
          name: 'reader',
          component: () => import('../view/Reader.vue'),
          meta: {
            requiresAuth: true
          },
        },
        {
          path: '/borrow',
          name: 'borrow',
          component: () => import('../view/Borrow.vue'),
          meta: {
            requiresAuth: true
          },
        },
        {
          path: '/book',
          name: 'book',
          component: () => import('../view/Book.vue'),
          meta: {
            requiresAuth: true
          },
        },
        {
          path: '/type',
          name: 'type',
          component: () => import('../view/Type.vue'),
          meta: {
            requiresAuth: true
          },
        },
        {
          path: '/logs',
          name: 'logs',
          component: () => import('../view/Logs.vue'),
          meta: {
            requiresAuth: true
          },
        },
      ]
    },
    {
      path: '/error',
      name: 'error',
      component: () => import('../view/error/Error.vue'),
    },
    {
      path: '*',
      redirect: '/error'
    }
  ]
})

router.beforeEach((to, from, next) => {
  if (to.meta.requiresAuth) {    //判断当前路由是否需要进行权限控制
    if (sessionStorage.getItem('code') == 200) {    //权限控制的具体规则
      if (to.name === 'logs' || to.name === 'user') {
        if (sessionStorage.getItem('UIdentity') != 0) {
          Message.error('无权访问');
          next({ path: '/main' })
        }
      }
      next()
    } else {
      Message.error('未登录，请登录');
      next({ path: '/' })
    }
  } else {
    next() // 放行
  }
})


export default router;
