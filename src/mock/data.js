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
  }, {
    bigType: '电脑',
    smallType: ['一体机', '台式机', '笔记本', '平板电脑', '瘦客户机']
  }, {
    bigType: '打印机',
    smallType: ['A3打印机', 'A4打印机', '条码打印机', '卡片打印机', '针式打印机']
  }, {
    bigType: '推车',
    smallType: ['推车']
  }, {
    bigType: '电视机',
    smallType: ['电视机']
  }, {
    bigType: '支架',
    smallType: ['支架']
  } ]
  return {
    code: 200,
    data: HardType,
    msg: ''
  }
}

export const getHardList = req => {
  let HardList = [{
    proLineName: '麻醉',
    bigType: '服务器',
    smallType: '服务器',
    hardBrand: '联想',
    hardModel: 'X3650M5',
    hardParams: [{
      dictType: 'CPU型号',
      dictName: 'E5-2620v4'
    }, {
      dictType: 'CPU核数',
      dictName: '2.1GHz'
    }, {
      dictType: 'CPU颗数',
      dictName: '1颗'
    }, {
      dictType: '内存',
      dictName: '2*16GB DDR4 2133MHz'
    }, {
      dictType: '硬盘',
      dictName: '3x600G 10K SAS'
    }, {
      dictType: '冗余电源',
      dictName: 'BM System x 550W High Efficiency '
    }, {
      dictType: '导轨',
      dictName: '2U 滑轨'
    }],
    hardWarranty: '2',
    warrantyType: '上门',
    warrantyPrice: '300',
    hardImg: '',
    IsEnergy: 1,
    hardPrice: '4000',
    CreateTime: '2018-08-08'
  }, {
    proLineName: '麻醉',
    bigType: '服务器',
    smallType: '服务器',
    hardBrand: '惠普',
    hardModel: 'DL388 Gen10',
    hardParams: [{
      dictType: 'CPU型号',
      dictName: 'E5-2620v4'
    }, {
      dictType: 'CPU核数',
      dictName: '2.4GHz'
    }, {
      dictType: 'CPU颗数',
      dictName: '1颗'
    }, {
      dictType: '内存',
      dictName: '2*16GB DDR4 2133MHz'
    }, {
      dictType: '硬盘',
      dictName: '3x600G 10K SAS'
    }, {
      dictType: '冗余电源',
      dictName: '500W*2'
    }, {
      dictType: '导轨',
      dictName: '2U 滑轨'
    }],
    hardWarranty: '2',
    warrantyType: '上门',
    warrantyPrice: '300',
    hardImg: ['http://image.tianjimedia.com/uploadImages/2018/220/20180806230936225001.jpg', 'http://img.pconline.com.cn/images/product/6246/624662/Z.jpg'],
    IsEnergy: 1,
    hardPrice: '4000',
    CreateTime: '2018-08-08'
  },
  {
    proLineName: '重症',
    bigType: '电脑',
    smallType: '台式机',
    hardBrand: '惠普',
    hardModel: 'ProDesk 600 G3 SFF',
    hardParams: [{
      dictType: '主板',
      dictName: '英特尔Q270芯片组'
    }, {
      dictType: 'CPU',
      dictName: 'Intel Core i5-6500'
    }, {
      dictType: '端口',
      dictName: '双网卡双串口'
    }, {
      dictType: '内存',
      dictName: '4G'
    }, {
      dictType: '硬盘',
      dictName: '128G SSD SATA硬盘'
    }, {
      dictType: '显卡',
      dictName: '集成显卡'
    }, {
      dictType: '显示器',
      dictName: '9.5"宽屏液晶 1600x900'
    }],
    hardWarranty: '2',
    warrantyType: '上门',
    warrantyPrice: '300',
    hardImg: '',
    IsEnergy: 0,
    hardPrice: '4000',
    CreateTime: '2018-08-08'
  }]
  return {
    code: 200,
    data: HardList,
    msg: ''
  }
}
