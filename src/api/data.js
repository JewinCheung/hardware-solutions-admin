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

export const hardType = {

  saveHardType (data) {
    return axios.request({
      url: 'HardType/SaveHardType',
      data,
      method: 'post'
    })
  },

  saveTypeConfig (data) {
    return axios.request({
      url: 'HardType/SaveTypeConfig',
      data,
      method: 'post'
    })
  },

  getHardType () {
    return axios.request({
      url: 'HardType/GetHardType/1001X21000000002JO17',
      method: 'get'
    })
  },

  getTypeConfig (id) {
    return axios.request({
      url: 'HardType/GeTypeConfig/' + id,
      method: 'get'
    })
  },

  getTypeInfo (id) {
    return axios.request({
      url: 'HardType/GetTypeInfo/' + id,
      method: 'get'
    })
  },

  getConfigInfo (id) {
    return axios.request({
      url: 'HardType/GetConfigInfo/' + id,
      method: 'get'
    })
  }

}

export const hardItem = {

  saveItem (data) {
    return axios.request({
      url: 'MaterialItem/SaveItem',
      data,
      method: 'post'
    })
  },

  getItemData ({PageIndex, PageSize}) {
    const data = {
      PageIndex, PageSize
    }
    return axios.request({
      url: 'MaterialItem/GetItem',
      params: data,
      method: 'get'
    })
  }

}
