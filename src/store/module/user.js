import { login, getUserInfo } from '@/api/user'
import { setToken, getToken } from '@/libs/util'
import { resData } from '@/libs/tools'
export default {
  state: {
    account: '',
    userName: '',
    userId: '',
    avatorImgPath: '',
    token: getToken(),
    access: ''
  },
  mutations: {
    setAvator (state, avatorPath) {
      state.avatorImgPath = avatorPath
    },
    setUserId (state, id) {
      state.userId = id
    },
    setUserName (state, name) {
      state.userName = name
    },
    setAccess (state, access) {
      state.access = access
    },
    setToken (state, token) {
      state.token = token
      setToken(token)
    }
  },
  actions: {
    // 登录
    handleLogin ({ commit }, {account, passwd}) {
      account = account.trim()
      return new Promise((resolve, reject) => {
        login({
          account,
          passwd
        }).then(res => {
            console.log(res)
          resData(
            res,
            res => {
              const data = res.Data
              console.log(res)
              console.log(data)
              commit('setToken', data)
             
            },
            res => {
           
            }
          )
          resolve(res)
        }).catch(err => {
          reject(err)
        })
      })
    },
    // 退出登录
    handleLogOut ({ state, commit }) {
      return new Promise((resolve, reject) => {
        // logout(state.token).then(() => {
        commit('setToken', '')
        commit('setAccess', [])
        resolve()
        // }).catch(err => {
        //   reject(err)
        // })
        // 如果你的退出登录无需请求接口，则可以直接使用下面三行代码而无需使用logout调用接口
        // commit('setToken', '')
        // commit('setAccess', [])
        // resolve()
      })
    },
    // 获取用户相关信息
    getUserInfo ({commit}) {
      return new Promise((resolve, reject) => {
        getUserInfo().then(res => {

          const data = res.Data.info
          console.log(res.Data.info);
          console.log(res.Data.Access);
          // console.log(data)
          // commit('setAvator', data.avator)
          commit('setUserName', data.UserName)
          // commit('setUserId', data.user_id)
          commit('setAccess',res.Data.Access)
          resolve(res)
        }).catch(err => {
          reject(err)
        })
      })
    }
  }
}
