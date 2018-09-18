import axios from '@/libs/api.request'

export const login = ({ account, passwd}) => {

  const data = {
    account,passwd,"device":"hard"
  }
  return axios.request({
    url: 'auth',
    params: data,
    method: 'get'
  })
}

export const getUserInfo = () => {
  return axios.request({
    url: 'UserInfo',
    method: 'get'
  })
}

export const logout = (token) => {
  return axios.request({
    url: 'logout',
    method: 'post'
  })
}
