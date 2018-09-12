<template>
  <div class="sales-hard-query">
    <Card>
      <div class="search-con search-con-top">
        <Input clearable search size="large" v-model="searchValue" placeholder="输入关键字搜索" style="width:80%" />
        <Button class="search-btn" type="primary" @click="handleSearch">
          <Icon type="search" />&nbsp;&nbsp;搜索</Button>

      </div>
      <div class="tag-nav-line">
        产品线：
        <Tag v-for="item in ProLine" :key="item" :name="item" :color="item === tag_name ? 'primary' : 'default'"
          @click.native="tagClick(item)">{{ item}}</Tag>
      </div>
      <div class="tag-nav-line">
        硬件分类：
        <Select v-model="bigType" style="width:160px" @on-change="getSmall">
          <Option v-for="item in HardType" :value="item.bigType" :key="item.bigType">{{ item.bigType }}</Option>
        </Select>
        <Tag v-for="item in smallType" :key="`tag-nav-${item}`" :name="item" color="default" @click.native="tagAdd(item)">{{
          item }}</Tag>
      </div>
      <div>
        <Tag closable v-for="item in tagCount" :key="item" :name="item.title" color="warning" @on-close="tagClose(item)">{{
          item}}</Tag>
      </div>
      <Table :data="hardList" :columns="hardColumns" stripe ref="tables"></Table>
      <div style="margin: 10px;overflow: hidden">
                <Button type="primary" size="large" @click="exportData()">
          <Icon type="ios-download-outline"></Icon>导出
        </Button>
        <div style="float: right;">
          <Page :total="1" :current="1" @on-change="changePage"></Page>
        </div>
      </div>
    </Card>
  </div>
</template>

<script>
  import Tables from '_c/tables'
  import {
    getProLine,
    getHardType,
    getHardList
  } from '@/api/data'
  export default {
    name: 'sales-hard-query',
    components: {
      Tables
    },
    data() {
      return {
        searchValue: '',
        ProLine: [], // 产品线
        HardType: [], // 硬件分类
        smallType: [], // 硬件小类
        tag_name: 'default', // 默认标签
        bigType: '', // 模式大类
        tagCount: [],
        hardList: [],
        hardColumns: []
      }
    },
    methods: {
      // 切换硬件小分类
      getsmallType() {
        const bigTypeName = this.bigType

        const HardType = this.HardType.filter(item => {
          return item.bigType === bigTypeName
        })

        setTimeout(() => {
          this.smallType = HardType[0].smallType
        }, 300)
      },
      // 标签点击事件
      tagClick(name) {
        this.tag_name = name
        this.tagAdd(name)
      },
      // 添加标签
      tagAdd(name) {
        const index = this.tagCount.indexOf(name)
        if (index < 0) {
          this.tagCount.push(name)
        }
      },
      // 关闭标签
      tagClose(name) {
        const index = this.tagCount.indexOf(name)
        this.tagCount.splice(index, 1)
      },
      // 硬件大分类选择事件
      getSmall(Option) {
        this.bigType = Option
        this.smallType = []
        this.getsmallType()
      },
      changePage() {},
      sethardColumns() {
        this.hardColumns = [{
            title: '产品线',
            key: 'proLineName'
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
                        this.hardList[params.index].hardParams.map(item => {
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
                    type: this.hardList[params.index].hardImg.length > 0 ? 'ios-eye' : '',
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
                  this.hardList[params.index].hardImg.length > 0 ? '点击预览' : '无图片信息'
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
        if (this.hardList[index].hardImg.length > 0) {
          this.$Modal.info({
            scrollable: true,
            closable: true,
            render: h => {
              return h(
                'div',
                this.hardList[index].hardImg.map(item => {
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
      handleSearch() {
        if (this.searchValue.length > 0) {
          this.tagAdd(this.searchValue)
        }
        // 硬件列表
        getHardList().then(res => {
          this.hardList = res.data
          if (this.tagCount.length > 0) {
            this.hardList = this.hardList.filter(item => {
              var isOk = false
              this.tagCount.map(i => {
                if (
                  item.proLineName.indexOf(i) > -1 ||
                  item.bigType.indexOf(i) > -1 ||
                  item.smallType.indexOf(i) > -1
                ) {
                  return (isOk = true)
                }
              })
              if (isOk) {
                return item
              }
            })
          }
        })
      },
      exportData() {
        this.$refs.tables.exportCsv({
          filename: `硬件列表-${(new Date()).valueOf()}.csv`,
          columns: this.hardColumns.filter((col, index) => index != 5),
          data: this.hardList.filter((data, index) => index != 5)
        })

      }
    },
    mounted() {
      // 初始化产品线
      getProLine().then(res => {
        this.ProLine = res.data
        // const Line = this.ProLine[0];
        // this.tag_name = Line;
        // this.tagClick(Line);
      })
      // 初始化硬件分类
      getHardType().then(res => {
        this.HardType = res.data
        this.bigType = this.HardType[0].bigType
        this.getsmallType()
      })
      // 硬件列表
      this.sethardColumns()
      this.handleSearch()
    }
  }

</script>

<style lang="less">
  .sales-hard-query {
    .search-con {
      padding: 0 0 10px 0;
    }

    .search-input {
      width: 600px;
    }

    .tag-nav-line {
      .ivu-tag {
        height: 28px;
        line-height: 28px;
        margin: 5px;
        border: 1;
      }

      .ivu-tag-primary {
        border: 1 !important;
      }
    }
  }

</style>
