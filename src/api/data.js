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

export const getInfoList = () => {
  return axios.request({
    url: 'getInfoList',
    method: 'get'
  })
}

export const productLine = {

  add (data) {
    return axios.request({
      url: 'ProLine/addProLine',
      data,
      method: 'post'
    })
  },
  edit (id, data) {
    return axios.request({
      url: 'ProLine/EditProLine/' + id,
      data,
      method: 'post'
    })
  },

  getData () {
    return axios.request({
      url: 'ProLine/GetProLine',
      method: 'get'
    })
  }

}
