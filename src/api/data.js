import axios from '@/libs/api.request'

export const getTableData = () => {
  return axios.request({
    url: 'get_table_data',
    method: 'get'
  })
}

export const getProLine = () => {
  return axios.request({
    url: 'getProLine',
    method: 'get'
  })
}


export const getHardType = () => {
  return axios.request({
    url: 'getHardType',
    method: 'get'
  })
}

export const getHardList = () => {
  return axios.request({
    url: 'getHardList',
    method: 'get'
  })
}



