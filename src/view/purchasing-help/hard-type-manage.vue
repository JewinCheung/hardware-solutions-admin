<template>
  <div>
    <Row :gutter="10">
      <Col span="16">
      <Card>
        <Row :gutter="10">
          <Col span="12">
          <Card dis-hover>
            <p slot="title">
              <Icon type="ios-paper-outline"></Icon>
              硬件大类
            </p>
            <ButtonGroup shape="circle" slot="extra">
              <Button icon="md-add" @click="addType('big')"></Button>
              <Button icon="md-create" @click="editType('big')"></Button>
              <Button icon="md-remove" disabled></Button>
            </ButtonGroup>
            <div style="height: 300px;">
              <ul class="iview-admin-draggable-list">
                <li v-for="(item, index) in hardTypeData" :key="item.SerialNo" :class="item.SerialNo==smallTypeData.SerialNo?'select':'notwrap'"
                  :data-index="index" @click="bigTypeClick(item)">
                  {{ item.MatTypeName }}
                </li>
              </ul>
            </div>
          </Card>
          </Col>
          <Col span="12" class="padding-left-10">
          <Card dis-hover>
            <p slot="title">
              <Icon type="ios-list"></Icon>
              硬件小类-[{{smallTypeData.MatTypeName}}]
            </p>
            <ButtonGroup shape="circle" slot="extra">
              <Button icon="md-add" @click="addType('small')"></Button>
              <Button icon="md-create" @click="editType('small')"></Button>
              <Button icon="md-remove" disabled></Button>
            </ButtonGroup>
            <div style="height: 300px;">
              <ul id="todoList" class="iview-admin-draggable-list">
                <li v-for="(item, index) in smallTypeData.smallType" :key="item.SerialNo" :class="item.SerialNo==smallTypeInfo.SerialNo?'select':'notwrap'"
                  :data-index="index" @click="smallTyClick(item)">
                  {{item.MatTypeName}}
                </li>
              </ul>
            </div>
          </Card>
          </Col>
        </Row>
      </Card>
      </Col>
      <Col span="8" class="padding-left-10">
      <Card>
        <p slot="title">
          <Icon type="ios-paper-outline"></Icon>
          硬件分类
        </p>
        <div style="height: 334px;">
          <div class="iview-admin-draggable-list">
            <Tree :data="typeTree"></Tree>
          </div>
        </div>
      </Card>
      </Col>
    </Row>
    <Row :gutter="10" style="margin-top: 10px;">
      <Col span="16">

      <Card>
        <Row :gutter="10">
          <Col span="12">
          <Card dis-hover id="Anchor_props">
            <p slot="title">
              <Icon type="ios-paper-outline"></Icon>
              硬件配置-[{{smallTypeData.MatTypeName}}]
            </p>
            <ButtonGroup shape="circle" slot="extra">
              <Button icon="md-add" @click="addConfig('config')"></Button>
              <Button icon="md-create" @click="editConfig('config')"></Button>
              <Button icon="md-remove" disabled></Button>
            </ButtonGroup>
            <div style="height: 300px;">
              <ul class="iview-admin-draggable-list">
                <li v-for="(item, index) in TypeConfigData" :key="item.SerialNo" :class="item.ItmeName==ConfigItmeData.ItmeName?'select':'notwrap'"
                  :data-index="index" @click="typeConfigClick(item)">
                  {{ item.ItmeName }}
                </li>
              </ul>
            </div>
          </Card>
          </Col>
          <Col span="12" class="padding-left-10">
          <Card dis-hover>
            <p slot="title">
              <Icon type="ios-list"></Icon>
              配置选项-[{{ConfigItmeData.ItmeName}}]
            </p>
            <ButtonGroup shape="circle" slot="extra">
              <Button icon="md-add" @click="addConfig('item')"></Button>
              <Button icon="md-create" @click="editConfig('item')"></Button>
              <Button icon="md-remove" disabled></Button>
            </ButtonGroup>
            <div style="height: 300px;">
              <ul id="todoList" class="iview-admin-draggable-list">
                <li v-for="(item, index) in ConfigItmeData.ConfigItme" :key="item.SerialNo" :class="item.ItmeName==ConfigItmeInfo.ItmeName?'select':'notwrap'"
                  :data-index="index" @click="ConfigItmeClick(item)">
                  {{ item.ItmeName }}
                </li>
              </ul>
            </div>
          </Card>
          </Col>
        </Row>
      </Card>
      </Col>
      <Col span="8" class="padding-left-10">
      <Card>
        <p slot="title">
          <Icon type="ios-paper-outline"></Icon>
          配置清单
        </p>
        <div style="height: 334px;">
          <div class="iview-admin-draggable-list">
            <Tree :data="configTree"></Tree>
          </div>
        </div>
      </Card>
      </Col>
    </Row>

    <Modal v-model="hardType_Modal">
      <p slot="header" style="text-align:center">

        <span>{{typeName.name}}</span>
      </p>
      <div>
        <Form ref="typeInfoForm" :model="typeForm" :rules="ruleValidate_Type" :label-width="100">
          <FormItem label="类别名称:" prop="MatTypeName">
            <Input v-model="typeForm.MatTypeName" />
          </FormItem>
          <FormItem label="说明:" prop="MatTypeMemo">
            <Input v-model="typeForm.MatTypeMemo" type="textarea" :autosize="{minRows: 2,maxRows: 5}" />
          </FormItem>
        </Form>

      </div>
      <div slot="footer">
        <Button type="primary" @click="handleTypeSubmit('typeInfoForm')" :loading="hardType_loading">保存</Button>
        <Button @click="hardType_Modal=false" style="margin-left: 8px">关闭</Button>
      </div>
    </Modal>

    <Modal v-model="config_Modal">
      <p slot="header" style="text-align:center">

        <span>{{typeName.name}}</span>
      </p>
      <div>
        <Form ref="configInfoForm" :model="configForm" :rules="ruleValidate_congfig" :label-width="100">
          <FormItem label="名称:" prop="ItmeName">
            <Input v-model="configForm.ItmeName" />
          </FormItem>
          <FormItem label="说明:" prop="Memo">
            <Input v-model="configForm.Memo" type="textarea" :autosize="{minRows: 2,maxRows: 5}" />
          </FormItem>
        </Form>

      </div>
      <div slot="footer">
        <Button type="primary" @click="handleConfigSubmit('configInfoForm')" :loading="hardType_loading">保存</Button>
        <Button @click="config_Modal=false" style="margin-left: 8px">关闭</Button>
      </div>
    </Modal>
  </div>
</template>

<script>
import { hardType } from '@/api/data'
import { resData } from '@/libs/tools'
export default {
  name: 'hard-type-manage',
  data () {
    return {
      hardType: [
        {
          bigType: '服务器',
          smallType: ['服务器']
        },
        {
          bigType: '电脑',
          smallType: ['一体机', '台式机', '笔记本', '平板电脑', '瘦客户机']
        },
        {
          bigType: '打印机',
          smallType: [
            'A3打印机',
            'A4打印机',
            '条码打印机',
            '卡片打印机',
            '针式打印机'
          ]
        },
        {
          bigType: '推车',
          smallType: ['推车']
        },
        {
          bigType: '电视机',
          smallType: ['电视机']
        },
        {
          bigType: '支架',
          smallType: ['支架']
        }
      ],
      paramsType: [
        {
          smallType: '服务器',
          paramName: 'CPU颗数',
          itemName: ['1颗', '2颗', '3颗', '4颗']
        },
        {
          smallType: '服务器',
          paramName: 'CPU核数',
          itemName: ['2.1GHz', '2.4GHz', '2.8GHz', '3.1GHz']
        },
        {
          smallType: '服务器',
          paramName: 'CPU型号',
          itemName: ['E5-2620v4', 'Intel Core i5-6500', 'I5 六代']
        },
        {
          smallType: '服务器',
          paramName: '内存',
          itemName: ['4G', '8G', '16G', '32G']
        },
        {
          smallType: '服务器',
          paramName: '硬盘',
          itemName: ['150G', '250G', '500G', '1T']
        },
        {
          smallType: '服务器',
          paramName: '操作系统',
          itemName: []
        }
      ],
      hardType_loading: false, // 按钮状态
      hardType_Modal: false, // 弹出框
      config_Modal: false, // 弹出框
      hardTypeData: [], // 类别集合[{大类,[小类,小类,小类]},{大类,[小类,小类,小类]}]
      smallTypeData: {}, // 详情，小类集合{大类,[小类,小类小类]}
      TypeConfigData: [], // 配置集合[{配置,[配置选项,配置选项]},{配置,[配置选项,配置选项]}]
      ConfigItmeData: {}, // 配置详情，选项集合{配置,[配置选项,配置选项]}
      smallTypeInfo: {}, // 类详情
      ConfigItmeInfo: {}, // 配置详情
      typeTree: [],
      configTree: [],
      typeName: {
        // 弹出框熟悉
        id: '', // big small  config item
        name: '', // title
        type: '' // add edit
      },
      typeForm: {
        // 硬件类别表单
        SerialNo: '',
        MatTypeName: '',
        MatTypeNo: '',
        ParentNo: '',
        MatTypeMemo: ''
      },
      configForm: {}, // 配置信息表单
      ruleValidate_Type: {
        // 类型表单验证
        MatTypeName: [
          {
            required: true,
            message: '不能为空'
          }
        ]
      },
      ruleValidate_congfig: {
        // 表单验证
        ItmeName: [
          {
            required: true,
            message: '不能为空'
          }
        ]
      }
    }
  },
  mounted () {
    this.getHardTypeData()
  },

  methods: {
    // 获取硬件类别
    getHardTypeData () {
      hardType.getHardType().then(res => {
        this.hardTypeData = res.Data
        this.getTypeTree()
      })
    },
    getTypeTree () {
      this.typeTree = this.hardTypeData.map(item => {
        return {
          title: item.MatTypeName,
          expand: true,
          children: this.getChildren(item.smallType, 'MatTypeName')
        }
      })
    },
    getConfigTree () {
      this.configTree = this.TypeConfigData.map(item => {
        return {
          title: item.ItmeName,
          expand: true,
          children: this.getChildren(item.ConfigItme, 'ItmeName')
        }
      })
    },
    getChildren (data, name) {
      var list = data.map(item => {
        return {
          title: item[name],
          expand: true
        }
      })

      return list
    },
    // 获取硬件配置
    getTypeConfigData (SerialNo) {
      this.ConfigItmeData = []
      hardType.getTypeConfig(SerialNo).then(res => {
        this.TypeConfigData = res.Data
        this.getConfigTree()
        if (res.Data.length === 0) {
          this.$Message.warning(this.smallTypeData.MatTypeName + '-无硬件配置')
        }
      })
    },
    // 大类点击事件
    bigTypeClick (item) {
      this.smallTypeData = item
      this.smallTypeInfo = {}
      this.ConfigItmeInfo = {}
      // 加载配置
      this.getTypeConfigData(item.SerialNo)
      if (!item.smallType || item.smallType.length === 0) {
        this.$Message.warning(item.MatTypeName + '-无硬件小类')
      }
    },
    // 小类点击事件
    smallTyClick (itme) {
      this.smallTypeInfo = itme
    },

    // 硬件配置点击事件
    typeConfigClick (item) {
      this.ConfigItmeData = item
      console.log(this.ConfigItmeData)
      this.ConfigItmeInfo = {}
      if (item.ConfigItme.length === 0) {
        this.$Message.warning(item.ItmeName + '-无配置选项')
      }
    },
    // 配置选项点击事件
    ConfigItmeClick (itme) {
      this.ConfigItmeInfo = itme
    },
    // 提交表单
    handleTypeSubmit (name) {
      var postData = this.typeForm
      this.$refs[name].validate(valid => {
        if (valid) {
          this.hardType_loading = true
          hardType.saveHardType(postData).then(res => {
            resData(
              res,
              res => {
                var postDataInfo = res.Data
                if (this.typeName.id === 'big') {
                  if (this.typeName.type === 'add') {
                    postDataInfo['smallType'] = []
                    this.hardTypeData.unshift(postDataInfo)
                  } else {
                    let NewData = []
                    NewData = this.hardTypeData.map(itme => {
                      var smallType = itme.smallType
                      if (itme.SerialNo === postDataInfo.SerialNo) {
                        itme = postDataInfo
                        itme['smallType'] = smallType
                      }
                      return itme
                    })
                    this.hardTypeData = NewData
                  }
                } else {
                  if (this.typeName.type === 'add') {
                    // console.log(this.smallTypeData.smallType);
                    this.smallTypeData.smallType.unshift(postDataInfo)
                  } else {
                    let NewData = []
                    NewData = this.smallTypeData.smallType.map(itme => {
                      if (itme.SerialNo === postDataInfo.SerialNo) {
                        itme = postDataInfo
                      }
                      return itme
                    })
                    this.smallTypeData.smallType = NewData
                  }
                }
                this.hardType_Modal = false

                this.$Message.success(res.Msg)
              },
              res => {
                this.$Message.error(res.Msg)
              }
            )

            this.hardType_loading = false
            this.getTypeTree()
            this.getConfigTree()
          })
        }
      })
    },

    addType (name) {
      this.$refs.typeInfoForm.resetFields() // 重置表单
      this.typeName.id = name
      this.typeName.type = 'add'
      if (name === 'big') {
        this.typeName.name = '硬件大类-新增'
        this.typeForm['ParentNo'] = '1001X21000000002JO17'
        this.typeForm['MatTypeNo'] = '04'
      }
      if (name === 'small') {
        if (!this.smallTypeData.SerialNo) {
          this.$Message.error('请选择硬件大类')
          return
        }
        this.typeName.name =
          '硬件小类-新增-所属大类[' + this.smallTypeData.MatTypeName + ']'
        this.typeForm['ParentNo'] = this.smallTypeData.SerialNo
        this.typeForm['MatTypeNo'] = this.smallTypeData.MatTypeNo
      }
      this.typeForm['SerialNo'] = ''
      this.hardType_Modal = true
    },
    editType (name) {
      this.typeName.id = name
      this.typeName.type = 'edit'
      var SerialNo = ''
      if (!this.smallTypeData.SerialNo) {
        this.$Message.error('请选择硬件大类')
        return
      }
      if (name === 'big') {
        this.typeName.name = '硬件大类-编辑'

        SerialNo = this.smallTypeData.SerialNo
      }

      if (name === 'small') {
        if (!this.smallTypeInfo.SerialNo) {
          this.$Message.error('请选择硬件小类')
          return
        }
        this.typeName.name =
          '硬件小类-编辑-所属大类[' + this.smallTypeData.MatTypeName + ']'
        SerialNo = this.smallTypeInfo.SerialNo
      }

      hardType.getTypeInfo(SerialNo).then(res => {
        if (res) {
          this.typeForm = res
        }
      })

      this.hardType_Modal = true
    },

    addConfig (name) {
      this.$refs.configInfoForm.resetFields() // 重置表单
      this.typeName.id = name
      this.typeName.type = 'add'
      if (!this.smallTypeData.SerialNo) {
        this.$Message.error('请选择硬件大类')
        return
      }
      if (name === 'config') {
        this.typeName.name = '硬件配置-新增'
        this.configForm['ParentNo'] = this.smallTypeData.SerialNo
      }
      if (name === 'item') {
        if (!this.ConfigItmeData.SerialNo) {
          this.$Message.error('请选择硬件配置')
          return
        }
        this.typeName.name =
          '配置选项-新增-所属配置[' + this.ConfigItmeData.ItmeName + ']'
        this.configForm['ParentNo'] = this.ConfigItmeData.SerialNo
      }
      this.configForm['SerialNo'] = ''
      this.configForm['IsEnabled'] = 0
      this.config_Modal = true
    },

    editConfig (name) {
      this.typeName.id = name
      this.typeName.type = 'edit'
      var SerialNo = ''

      if (name === 'config') {
        if (!this.ConfigItmeData.SerialNo) {
          this.$Message.error('请选择硬件配置')
          return
        }
        this.typeName.name = '硬件配置-编辑'

        SerialNo = this.ConfigItmeData.SerialNo
      }

      if (name === 'item') {
        if (!this.ConfigItmeInfo.SerialNo) {
          this.$Message.error('请选择配置选项')
          return
        }
        this.typeName.name =
          '配置选项-编辑-所属配置[' + this.ConfigItmeData.ItmeName + ']'
        SerialNo = this.ConfigItmeInfo.SerialNo
      }

      hardType.getConfigInfo(SerialNo).then(res => {
        if (res) {
          this.configForm = res
          this.configForm['IsEnabled'] = 0
        }
      })
      this.config_Modal = true
    },

    handleConfigSubmit (name) {
      var postData = this.configForm
      this.$refs[name].validate(valid => {
        if (valid) {
          this.hardType_loading = true
          hardType.saveTypeConfig(postData).then(res => {
            resData(
              res,
              res => {
                var postDataInfo = res.Data
                if (this.typeName.id === 'config') {
                  if (this.typeName.type === 'add') {
                    postDataInfo['ConfigItme'] = []
                    this.TypeConfigData.unshift(postDataInfo)
                  } else {
                    let NewData = []
                    NewData = this.TypeConfigData.map(itme => {
                      var ConfigItme = itme.ConfigItme
                      if (itme.SerialNo === postDataInfo.SerialNo) {
                        itme = postDataInfo
                        itme['ConfigItme'] = ConfigItme
                      }
                      return itme
                    })
                    this.TypeConfigData = NewData
                  }
                } else {
                  if (this.typeName.type === 'add') {
                    // console.log(this.smallTypeData.smallType);
                    this.ConfigItmeData.ConfigItme.unshift(postDataInfo)
                  } else {
                    let NewData = []
                    NewData = this.ConfigItmeData.ConfigItme.map(itme => {
                      if (itme.SerialNo === postDataInfo.SerialNo) {
                        itme = postDataInfo
                      }
                      return itme
                    })
                    this.ConfigItmeData.ConfigItme = NewData
                  }
                }
                this.config_Modal = false

                this.$Message.success(res.Msg)
              },
              res => {
                this.$Message.error(res.Msg)
              }
            )

            this.hardType_loading = false
            this.getTypeTree()
            this.getConfigTree()
          })
        }
      })
    }
  }
}
</script>
<style lang='less' scoped>
.iview-admin-draggable-list {
  height: 100%;
}

.iview-admin-draggable-list li {
  padding: 9px;
  border: 1px solid #e7ebee;
  border-radius: 3px;
  margin-bottom: 5px;
  cursor: pointer;
  position: relative;
  transition: all 0.2s;
}

.iview-admin-draggable-list li:hover {
  // color: #87b4ee;
  border-color: #87b4ee;
  transition: all 0.2s;
}

.iview-admin-draggable-list .select {
  color: #fff;
  background-color: #57c5f7;
}

.placeholder-style {
  display: block !important;
  color: transparent;
  border-style: dashed !important;
}

.delte-item-animation {
  opacity: 0;
  transition: all 0.2s;
}

.iview-admin-draggable-list {
  overflow: auto;
}
</style>
