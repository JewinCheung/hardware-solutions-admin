<template>
  <div class="hrad-solutions-manage">
    <Row :gutter="20">
      <Col>
        <Card>
          <Row :gutter="10" style="margin-bottom: 10px;">
            <Col span="16">
              <Card>
                <div style="margin-bottom: 10px;">产品线：
                  <Select
                    style="width:160px"
                    v-model="ProLineInfo.value"
                    @on-change="proChange"
                    :label-in-value="true"
                  >
                    <Option
                      v-for="item in ProLine"
                      :value="item.DictNo"
                      :key="item.DictNo"
                    >{{ item.DictName }}</Option>
                  </Select>
                  <Button class="search-btn" type="primary" @click="setSmall">
                    <Icon type="ios-download-outline" size="16"/>&nbsp;&nbsp;保存方案
                  </Button>
                </div>
                <Transfer
                  :data="hardList"
                  :target-keys="targetKeys"
                  :render-format="renderTarget"
                  @on-change="handleChange"
                  :list-style="listStyle"
                  filterable
                ></Transfer>
              </Card>
            </Col>
            <Col span="8" class="padding-left-10">
              <Card>
                <p slot="title">
                  <Icon type="ios-paper-outline"></Icon>
                  分配方案-[{{ProLineInfo.label}}]
                </p>
                <div style="height: 325px;">
                  <ul class="iview-admin-draggable-list">
                    <li
                      v-for="(item, index) in smallType"
                      :key="item.GroupNo"
                      class="notwrap todolist-item"
                      :data-index="index"
                      @click="smallTypeClick(item)"
                    >{{item.GroupType}}</li>
                  </ul>
                </div>
              </Card>
            </Col>
          </Row>

          <Row :gutter="10">
            <Col>
              <div class="tools">
                <ButtonGroup>
                  <Button type="primary" @click="AddNotGroupByItem">保存</Button>
                </ButtonGroup>
              </div>
              <Table
                @on-selection-change="selectChange"
                :data="hardItmeList"
                :columns="ItmeColumns"
                stripe
                ref="tables"
                height="400"
              ></Table>
              <div style="margin: 10px;overflow: hidden">
                <div style="float: right;">
                  <Page :total="1" :current="1" @on-change="changePage"></Page>
                </div>
              </div>
            </Col>
          </Row>
        </Card>
      </Col>
    </Row>

    <Modal v-model="itemConfig_Modal" width="60%">
      <p slot="header" style="text-align:center">
        <span>{{titleName.name}}</span>
      </p>
      <div>
        <Form ref="configformValidate" :label-width="120">
          <FormItem
            v-for="item in configData"
            :key="item.SerialNo"
            :label="item.ItmeName+'：'"
          >
            <Radio v-model="Radio">{{item.SubItmeName}}</Radio>
          </FormItem>
        </Form>
      </div>
      <div slot="footer">
        <Button @click="itemConfig_Modal=false" style="margin-left: 8px">关闭</Button>
      </div>
    </Modal>
  </div>
</template>

<script>
import { productLine, hardType, solutions } from '@/api/data'
import { resData } from '@/libs/tools'
export default {
  name: 'hrad-solutions-manage',
  data () {
    return {
      hardList: [], // 所有硬件类目
      targetKeys: [], // 产品线下已有类目
      ProLine: [], // 所有产品线
      ProLineInfo: {
        // 当前选中产品线
        value: '', // 产品线编号
        label: '未选择' // 产品线名称
      },
      smallType: [], // 已分配硬件类目
      SubItemNo: [], // 已选分类
      // lineName: '未选择',
      hardItmeList: [], // 所选硬件类目下硬件列表
      ItmeColumns: [], // 硬件列表展示字段
      listStyle: {
        width: '45%',
        height: '334px'
      },
      Radio: true,
      configData: [{
        SerialNo: '',
        ItmeName: '',
        SubItmeName: ''
      }],
      onSelect: {
        // 当前选中行
        index: 0,
        row: null
      },
      selectionAll: [], // 表格所以选中项
      itemConfig_Modal: false, // 硬件配置弹出框
      titleName: {
        // 弹出框展现信息
        // 弹出框熟悉
        id: '', // big small  config item
        name: '', // title
        type: '' // add edit
      }
    }
  },
  mounted () {
    // 硬件列表
    // this.getMockData()
    this.getProLineData()
    this.sethardColumns()
    // this.getHardItemData();
    // this.sethardColumns();
  },
  methods: {
    // 获取产品线
    getProLineData () {
      // 初始化产品线
      productLine.getProLine().then(res => {
        this.ProLine = res
      })
    },

    // 选择/变更产品线
    proChange (Option) {
      this.ProLineInfo = Option
      this.targetKeys = []
      this.smallType = []
      this.hardList = []
      this.hardItmeList = []
      this.SubItemNo = []
      this.getHardTypeData()
    },
    // 获取硬件类别
    getHardTypeData () {
      hardType.getHardType().then(res => {
        var hardList = res.Data
        var typeData = []
        console.log(hardList)
        hardList.map(item => {
          var bigType = item.MatTypeName

          item.smallType.map((i, index) => {
            return typeData.push({
              key: i.MatTypeNo,
              label: i.MatTypeName,
              type: bigType
            })
          })
        })
        this.hardList = typeData
        var ProLineNo = this.ProLineInfo.value
        this.getGroupProLine(ProLineNo)
      })
    },
    // 获取产品线下已存在的硬件类目
    getGroupProLine (ProLineNo) {
      console.log(ProLineNo)
      solutions.getGroupProLine(ProLineNo).then(res => {
        if (res.length > 0) {
          this.smallType = res
          var smallType = this.smallType.map(item => item.GroupNo)
          this.targetKeys = smallType
        }
      })
    },
    handleChange (newTargetKeys) {
      this.targetKeys = newTargetKeys
    },
    renderTarget (item) {
      return '[' + item.type + ']-' + item.label
    },
    // 保存方案
    setSmall () {
      // this.smallType = this.hardList.filter(item => (item.key = 10));
      var ProLineNo = this.ProLineInfo.value
      if (this.targetKeys.length === 0) {
        this.$Message.warning('目的列表无数据！')
        return
      }
      if (this.ProLineInfo.label === '未选择') {
        this.$Message.warning('请选择产品线！')
        return
      }
      var typeData = []
      this.hardList.filter(item => {
        var isOk = false
        this.targetKeys.map(i => {
          if (item.key === i) {
            return (isOk = true)
          }
        })
        console.log(item)
        if (isOk) {
          return typeData.push({
            GroupNo: item.key,
            GroupType: item.label,
            ProLineNo: ProLineNo
          })
        }
      })
      this.smallType = typeData
      console.log(this.smallType)

      solutions.setGroupProLine(ProLineNo, typeData).then(res => {
        resData(
          res,
          res => {
            this.$Message.success(res.Msg)
          },
          res => {
            this.$Message.error(res.Msg)
          }
        )
      })
    },
    // 获取类目下的硬件
    smallTypeClick (item) {
      this.selectionAll = []
      var ProLineNo = item.ProLineNo
      var SubItemNo = item.GroupNo
      this.SubItemNo = item
      solutions.getGroupItemData(ProLineNo, SubItemNo).then(res => {
        this.hardItmeList = res.map(item => {
          if (item.ConfigDesc) {
            item.ConfigDesc = JSON.parse(item.ConfigDesc)
          } else {
            item.ConfigDesc = []
          }
          if (item._checked) {
            // 已选项目
            this.selectionAll.push(item)
          }

          return item
        })

        // this.$refs.tables.$refs.tbody.objData[2].isChecked= true;
      })
    },
    sethardColumns () {
      this.ItmeColumns = [
        {
          type: 'selection',
          width: 45,
          align: 'center',
          fixed: 'left'
        },
        {
          title: '大类',
          key: 'MatTypeName',
          sortable: true,
          width: 100,
          fixed: 'left'
        },
        {
          title: '子类',
          key: 'SubType',
          sortable: true,
          width: 100,
          fixed: 'left'
        },
        {
          title: '品牌',
          key: 'Brand',
          width: 100,
          sortable: true,
          fixed: 'left'
        },
        {
          title: '型号',
          key: 'ItemModel',
          width: 100,
          fixed: 'left',
          tooltip: true
        },
        {
          title: '参数',
          key: 'ConfigDesc',
          fixed: 'left',
          width: 80,
          render: (h, params) => {
            return h('tooltip', [
              h('Icon', {
                props: {
                  type:
                    this.hardItmeList[params.index].ConfigDesc.length > 0
                      ? 'ios-eye'
                      : '',
                  size:
                    this.hardItmeList[params.index].ConfigDesc.length > 0
                      ? '24'
                      : '24'
                },
                on: {
                  click: () => {
                    this.onSelect.row = params.row
                    this.getItemConfig()
                  }
                }
              }),
              h(
                'div',
                {
                  slot: 'content'
                },
                this.hardItmeList[params.index].ConfigDesc.length > 0
                  ? '点击查看/编辑'
                  : '点击设置'
              )
            ])
          }
        },
        {
          title: '质保期限',
          key: 'WarrantyPeriod',
          width: 100,
          align: 'center',
          render: (h, params) => {
            const text = params.row.WarrantyPeriod
              ? params.row.WarrantyPeriod + '/年'
              : ''
            return h('div', text)
          }
        },
        {
          title: '质保方式',
          key: 'warrantyType',
          width: 90,
          align: 'center'
        },
        {
          title: '延保费用',
          key: 'WarrantyCost',
          width: 90,
          align: 'center'
        },
        {
          title: '图片',
          key: '',
          width: 60,
          align: 'center',
          render: (h, params) => {
            return h('tooltip', [
              h('Icon', {
                props: {
                  type: this.hardItmeList[params.index].AttachFiles
                    ? 'ios-eye'
                    : '',
                  size: '24'
                },
                style: {},
                on: {
                  click: () => {
                    this.show(params.index)
                  }
                }
              }),

              h(
                'div',
                {
                  slot: 'content'
                },
                this.hardItmeList[params.index].AttachFiles
                  ? '点击预览'
                  : '无图片信息'
              )
            ])
          }
        },
        {
          title: '节能环保',
          key: 'IsEnergy',
          width: 90,
          align: 'center',
          render: (h, params) => {
            const row = params.row
            const text = row.IsEnergy === 1 ? '是' : '否'
            return h('div', text)
          }
        },
        {
          title: '采购价',
          width: 90,
          key: 'PurchasePrice',
          align: 'center'
        },
        {
          title: '建议价',
          width: 90,
          key: 'Offer',
          align: 'center'
        },
        {
          title: '周期',
          key: 'DeliveryDay',
          width: 90,
          align: 'center',
          render: (h, params) => {
            const text = params.row.DeliveryDay
              ? params.row.DeliveryDay + '/工作日'
              : ''
            return h('div', text)
          }
        },
        {
          title: '更新日期',
          key: 'LastTime',
          minWidth: 110,
          sortable: true
        }
      ]
    },
    // onRowClick(row,index) {
    //   console.log(this.$refs.tables.$refs.tbody.objData[2]._isChecked);
    //   this.$refs.tables.$refs.tbody.objData[2]._isChecked= true;
    // },
    // 获取选中项
    selectChange (selection) {
      this.selectionAll = selection
      console.log(selection)
    },
    // 保存硬件列表
    AddNotGroupByItem () {
      var ProLineNoAndSubItemNo =
        this.ProLineInfo.value + ',' + this.SubItemNo.GroupNo
      var ProLineNo = this.ProLineInfo.value
      // 排除已选项
      var OutItem = []

      OutItem = this.hardItmeList.filter(Item => {
        var isOk = true
        this.selectionAll.map(i => {
          if (i.SerialNo === Item.SerialNo) {
            return (isOk = false)
          }
        })
        return isOk
      })
      OutItem = OutItem.map(item => {
        return {
          ProLineNo: ProLineNo,
          SubItemNo: item.SubItemNo,
          ItemNo: item.ItemNo
        }
      })
      solutions.setNotGroupItem(ProLineNoAndSubItemNo, OutItem).then(res => {
        resData(
          res,
          res => {
            this.$Message.success(res.Msg)
          },
          res => {
            this.$Message.error(res.Msg)
          }
        )
      })

      console.log(OutItem)
    },
    // 查看硬件配置
    getItemConfig () {
      if (!this.onSelect.row) {
        this.$Message.error('请选择行！')
        return
      }
      let rows = this.onSelect.row

      this.configData = []
      this.titleName.name =
        '[' + rows.SubType + '-' + rows.ItemModel + ']配置硬件信息'
      let ConfigDesc = this.onSelect.row.ConfigDesc

      if (ConfigDesc) {
        this.configData = ConfigDesc
        console.log(this.configData)
      }
      this.itemConfig_Modal = true
    },
    show (index) {
      if (this.hardItmeList[index].AttachFiles) {
        var AttachFiles = JSON.parse(this.hardItmeList[index].AttachFiles)

        this.$Modal.info({
          scrollable: true,
          closable: true,
          render: h => {
            return h(
              'div',
              AttachFiles.map(item => {
                return h('img', {
                  attrs: {
                    src: item.url
                  },
                  style: {
                    width: '100%'
                  }
                })
              })
            )
          }
        })
      }
    },

    /// /////////////////////////////////////////////////////////////////////////分割线
    getMockData () {
      productLine.getHardType().then(res => {
        var hardList = res.data
        var typeData = []
        hardList.map(item => {
          var bigType = item.bigType

          item.smallType.map((i, index) => {
            return typeData.push({
              key: typeData.length,
              label: i,
              type: bigType
            })
          })
        })
        this.hardList = typeData
        this.getTargetKeys()
        // console.log(this.hardList);
      })
    },
    getTargetKeys () {
      var hardList = this.hardList
        .filter((i, inedx) => inedx > 10)
        .map(item => item.key)
      this.targetKeys = hardList
    },

    OldsetSmall () {
      // this.smallType = this.hardList.filter(item => (item.key = 10));

      if (this.targetKeys.length === 0) {
        this.$Message.warning('目的列表无数据！')
        return
      }
      if (this.lineName === '未选择') {
        this.$Message.warning('请选择产品线！')
        return
      }

      this.smallType = this.hardList.filter(item => {
        var isOk = false
        this.targetKeys.map(i => {
          if (item.key === i) {
            return (isOk = true)
          }
        })
        if (isOk) {
          return item
        }
      })
    },

    getHardItemData () {
      // 硬件列表
      getHardList().then(res => {
        this.hardItmeList = res.data
        console.log(this.hardItmeList)
        // if (this.smallType.length > 0) {
        // this.hardItmeList = this.hardItmeList.filter(item => {
        //   var isOk = false;
        //   this.smallType.map(i => {
        //     if (item.label.indexOf(i) > -1) {
        //       return (isOk = true);
        //     }
        //   });
        //   if (isOk) {
        //     return item;
        //   }
        // });
        // }
      })
    },
    changePage () {}
  }
}
</script>
<style lang="less">
.tools {
  margin: 0 0 5 0;

  button {
    margin: 5px;
  }
}

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
  color: #87b4ee;
  border-color: #87b4ee;
  transition: all 0.2s;
}

.iview-admin-draggable-list {
  overflow: auto;
}
</style>
