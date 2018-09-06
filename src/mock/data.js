import Mock from 'mockjs'
import {
  doCustomTimes
} from '@/libs/util'

export const getTableData = req => {
  let tableData = []
  doCustomTimes(5, () => {
    tableData.push(Mock.mock({
      name: '@name',
      email: '@email',
      createTime: '@date'
    }))
  })
  return {
    code: 200,
    data: tableData,
    msg: ''
  }
}

export const getProLine = req => {
  let ProLine = ['麻醉', '重症', '数字化', '重症V6.0', '麻醉一体机', '移动护理']
  return {
    code: 200,
    data: ProLine,
    msg: ''
  }
}


export const getHardType = req => {
  let HardType = [{
    bigType: '服务器',
    smallType: ['服务器']
  },{
    bigType: '电脑',
    smallType: ['一体机','台式机','笔记本','平板电脑','瘦客户机']
  },{
    bigType: '打印机',
    smallType: ['A3打印机','A4打印机','条码打印机','卡片打印机','针式打印机']
  },{
    bigType: '推车',
    smallType: ['推车']
  },{
    bigType: '电视机',
    smallType: ['电视机']
  },{
    bigType: '支架',
    smallType: ['支架']
  },]
  return {
    code: 200,
    data: HardType,
    msg: ''
  }
}
