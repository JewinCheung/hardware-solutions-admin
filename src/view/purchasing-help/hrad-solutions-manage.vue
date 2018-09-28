<template>
  <div class="hrad-solutions-manage">

    <Row :gutter="20">
      <Col>
      <Card>
        <Row :gutter="10" style="margin-bottom: 10px;">
          <Col span="16">
          <Card>
            <div style="margin-bottom: 10px;">
              产品线：
              <Select style="width:160px" v-model="lineName" @on-change="proChange">
                <Option v-for="item in ProLine" :value="item.DictNo" :key="item">{{ item.DictName }}</Option>
              </Select>
              <Button class="search-btn" type="primary" @click="setSmall">
                <Icon type="ios-download-outline" size="16" />&nbsp;&nbsp;保存方案</Button>
            </div>
            <Transfer :data="hardList" :target-keys="targetKeys" :render-format="renderTarget" @on-change="handleChange"
              :list-style="listStyle" filterable></Transfer>
          </Card>
          </Col>
          <Col span="8" class="padding-left-10">
          <Card>
            <p slot="title">
              <Icon type="ios-paper-outline"></Icon>
              分配方案-[{{lineName}}]
            </p>
            <div style="height: 325px;">
              <ul class="iview-admin-draggable-list">
                <li v-for="(item, index) in smallType" :key="item.key" class="notwrap todolist-item" :data-index="index"
                  @click="smallTypeClick(item)">
                  {{item.label}}
                </li>
              </ul>
            </div>
          </Card>
          </Col>
        </Row>

        <Row :gutter="10">

          <Col>
          <div class="tools">
            <ButtonGroup>
              <Button type="primary">保存</Button>
            </ButtonGroup>
          </div>
          <Table :data="hardItmeList" :columns="ItmeColumns" stripe ref="tables"></Table>
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
  </div>
</template>

<script>
  import {
    productLine,
  } from '@/api/data'
  export default {
    name: 'hrad-solutions-manage',
    data() {
      return {
        hardList: [],
        targetKeys: [],
        ProLine: [], // 产品线
        smallType: [],
        lineName: '未选择',
        hardItmeList: [],
        ItmeColumns: [],
        listStyle: {
          width: '45%',
          height: '334px'
        }
      }
    },
    mounted() {
      // 硬件列表
      // this.getMockData()
      this.getProLineData();
      // this.getHardItemData();
      // this.sethardColumns();
    },
    methods: {
      getProLineData() {
        // 初始化产品线
        productLine.getData().then(res => {
          this.ProLine = res
        })
      },
      proChange(Option) {
        this.lineName = Option
        this.targetKeys = []
      },
      getMockData() {
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
      getTargetKeys() {
        var hardList = this.hardList
          .filter((i, inedx) => inedx > 10)
          .map(item => item.key)
        this.targetKeys = hardList
      },
      handleChange(newTargetKeys) {
        this.targetKeys = newTargetKeys
      },
      renderTarget(item) {
        return item.label + '-[' + item.type + ']'
      },
      setSmall() {
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
      sethardColumns() {
        this.ItmeColumns = [{
            type: 'selection',
            width: 60,
            align: 'center'
          },
          {
            title: '大类',
            key: 'bigType'
          },
          {
            title: '子类',
            key: 'smallType'
          },
          {
            title: '品牌',
            key: 'hardBrand'
          },
          {
            title: '型号',
            key: 'hardModel',
            tooltip: true
          },
          {
            title: '参数',
            key: 'hardParams',
            render: (h, params) => {
              return h(
                'tooltip', {
                  props: {
                    placement: 'top'
                  }
                },
                [
                  h('Tag', '···'),
                  h(
                    'div', {
                      slot: 'content'
                    },
                    [
                      h(
                        'ul',
                        this.hardItmeList[params.index].hardParams.map(item => {
                          return h(
                            'li', {
                              style: {
                                textAlign: 'left',
                                padding: '4px',
                                'list-style-type': 'none'
                              }
                            },
                            item.dictType + '：' + item.dictName
                          )
                        })
                      )
                    ]
                  )
                ]
              )
            }
          },
          {
            title: '质保',
            key: 'hardWarranty',
            width: 60,
            align: 'center'
          },
          {
            title: '质保类型',
            key: 'warrantyType',
            width: 90,
            align: 'center'
          },
          {
            title: '延保费用',
            key: 'warrantyPrice',
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
                    type: this.hardItmeList[params.index].hardImg.length > 0 ?
                      'ios-eye' : '',
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
                  'div', {
                    slot: 'content'
                  },
                  this.hardItmeList[params.index].hardImg.length > 0 ?
                  '点击预览' :
                  '无图片信息'
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
              return h('Tag', text)
            }
          },
          {
            title: '价格',
            key: 'hardPrice'
          },
          {
            title: '更新日期',
            key: 'CreateTime',
            width: 100
          }
        ]
      },
      show(index) {
        if (this.hardItmeList[index].hardImg.length > 0) {
          this.$Modal.info({
            scrollable: true,
            closable: true,
            render: h => {
              return h(
                'div',
                this.hardItmeList[index].hardImg.map(item => {
                  return h('img', {
                    attrs: {
                      src: item
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
      getHardItemData() {
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
      changePage() {}
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
