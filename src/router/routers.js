import Main from '@/view/main'
import parentView from '@/components/parent-view'

/**
 * iview-admin中meta除了原生参数外可配置的参数:
 * meta: {
 *  hideInMenu: (false) 设为true后在左侧菜单不会显示该页面选项
 *  notCache: (false) 设为true后页面不会缓存
 *  access: (null) 可访问该页面的权限数组，当前路由设置的权限会影响子路由
 *  icon: (-) 该页面在左侧菜单、面包屑和标签导航处显示的图标，如果是自定义图标，需要在图标名称前加下划线'_'
 * }
 */

export default [
  {
    path: '/login',
    name: 'login',
    meta: {
      title: 'Login - 登录',
      hideInMenu: true
    },
    component: () => import('@/view/login/login.vue')
  },
  {
    path: '/',
    name: 'home',
    redirect: '/home',
    component: Main,
    meta: {
      hideInMenu: true,
      notCache: true
    },
    children: [
      {
        path: '/home',
        name: 'home',
        meta: {
          hideInMenu: true,
          title: '首页',
          notCache: true
        },
        component: () => import('@/view/single-page/home')
      }
    ]
  },
  {
    path: '/sales-help',
    name: 'sales-help',
    meta: {
      icon: 'logo-buffer',
      title: '销售人员自助',
    },
    component: Main,
    children: [
      {
        path: 'hard-query',
        name: 'sales-hard-query',
        meta: {
          icon: 'md-trending-up',
          title: '销售-硬件库查询'
        },
        component: () => import('@/view/sales-help/hard-query.vue')
      },
      {
        path: 'information-feedback',
        name: 'sales-information-feedback',
        meta: {
          icon: 'md-grid',
          title: '销售-现场信息反馈'
        },
        component: () => import('@/view/sales-help/information-feedback.vue')
      }
    ]
  },
  {
    path: '/product-help',
    name: 'product-help',
    meta: {
      icon: 'md-menu',
      title: '产品人员自助',
    },
    component: Main,
    children: [
      {
        path: 'hard-query',
        name: 'product-hard-query',
        meta: {
          icon: 'md-trending-up',
          title: '产品-硬件库查询'
        },
        component: () => import('@/view/product-help/hard-query.vue')
      },
      {
        path: 'information-feedback',
        name: 'product-information-feedback',
        meta: {
          icon: 'md-grid',
          title: '产品-现场信息反馈'
        },
        component: () => import('@/view/product-help/information-feedback.vue')
      }

    ]
  },
  {
    path: '/purchasing-help',
    name: 'purchasing-help',
    meta: {
      icon: 'md-menu',
      title: '采购人员自助'
    },
    component: Main,
    children: [
      {
        path: 'hard-query',
        name: 'purchasing-hard-query',
        meta: {
          icon: 'md-trending-up',
          title: '采购-硬件库查询',
          hideInMenu: true,
        },
        component: () => import('@/view/purchasing-help/hard-query.vue')
      },
      {
        path: 'information-feedback',
        name: 'purchasing-information-feedback',
        meta: {
          icon: 'md-grid',
          title: '采购-现场信息反馈',
          hideInMenu: true,
        },
        component: () => import('@/view/purchasing-help/information-feedback.vue')
      },
      {
        path: '/proline-manage',
        name: 'proline-manage',
        meta: {
          icon: 'md-grid',
          title: '产品线维护'
        },
        component: () => import('@/view/purchasing-help/proline-manage.vue')
      },
      {
        path: '/hard-type-manage',
        name: 'hard-type-manage',
        meta: {
          icon: 'md-grid',
          title: '硬件分类维护'
        },
        component: () => import('@/view/purchasing-help/hard-type-manage.vue')
      },
      {
        path: '/hard-item-manage',
        name: 'hard-item-manage',
        meta: {
          icon: 'md-grid',
          title: '硬件信息维护'
        },
        component: () => import('@/view/purchasing-help/hard-item-manage.vue')
      },
      {
        path: '/hrad-solutions-manage',
        name: 'hrad-solutions-manage',
        meta: {
          icon: 'md-grid',
          title: '硬件方案分配'
        },
        component: () => import('@/view/purchasing-help/hrad-solutions-manage.vue')
      }
      
      

    ]
  },
  {
    path: '/401',
    name: 'error_401',
    meta: {
      hideInMenu: true
    },
    component: () => import('@/view/error-page/401.vue')
  },
  {
    path: '/500',
    name: 'error_500',
    meta: {
      hideInMenu: true
    },
    component: () => import('@/view/error-page/500.vue')
  },
  {
    path: '*',
    name: 'error_404',
    meta: {
      hideInMenu: true
    },
    component: () => import('@/view/error-page/404.vue')
  }
]
