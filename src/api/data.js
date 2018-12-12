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
  },
  getProLine () {
    return axios.request({
      url: 'ProLine/GetProLineByIsEnabled',
      method: 'get'
    })
  },

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

  getItemData ({PageIndex, PageSize,QueryKey}) {
    const data = {
      PageIndex, PageSize,QueryKey
    }
    return axios.request({
      url: 'MaterialItem/GetItem',
      params: data,
      method: 'get'
    })
  },

  setConfig (data) {
    return axios.request({
      url: 'MaterialItem/SetConfig',
      data,
      method: 'post'
    })
  },
  

}

export const solutions={

  //获取产品线对应的硬件类目
  getGroupProLine (id) {
    return axios.request({
      url: 'Solutions/GroupByProLine/'+id,
      method: 'get'
    })
  },

  ///保存产品线对应的硬件类目
  setGroupProLine (id,data) {
    return axios.request({
      url: 'Solutions/AddSolutions/'+id,
      data,
      method: 'post'
    })
  },

  //获取产品线对应的硬件类目下的硬件列表
  getGroupItemData (ProLineNo,SubItemNo) {
    const data = {
      ProLineNo:ProLineNo, 
      SubItemNo:SubItemNo
    }
    return axios.request({
      url: 'Solutions/GetItemGroupByProLine',
      params: data,
      method: 'get'
    })
  },

  //设置产品线对应的硬件类目下排除展示的硬件
  setNotGroupItem (id,data) {
    return axios.request({
      url: 'Solutions/AddNotGroupByItem/'+id,
      data,
      method: 'post'
    })
  },

}