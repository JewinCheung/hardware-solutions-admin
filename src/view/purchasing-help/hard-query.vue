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
        <Tag v-for="item in ProLine" :key="item.DictNo" :name="item.DictName" :color="item.DictName === tag_name ? 'primary' : 'default'"
          @click.native="tagClick(item)">{{ item.DictName}}</Tag>
      </div>
      <div class="tag-nav-line">
        硬件分类：
        <Select v-model="bigTypeNo" style="width:160px" @on-change="getSmall">
          <Option v-for="item in hardTypeData" :value="item.SerialNo" :key="item.SerialNo">{{ item.MatTypeName }}</Option>
        </Select>
        <Tag v-for="item in smallType" :key="item.SerialNo" :name="item.MatTypeName" color="default" @click.native="tagAdd(item.MatTypeName)">{{
          item.MatTypeName }}</Tag>
      </div>
      <div style="height:22px;margin: 5px 0 5px 0;">
        <Tag closable v-for="item in tagCount" :key="item" :name="item.title" color="warning" @on-close="tagClose(item)">{{
          item}}</Tag>
      </div>

      <Table :data="hardItmeList" :columns="ItmeColumns" stripe ref="tables" height="480"></Table>
      <div style="margin: 10px;overflow: hidden">
        <Button type="primary" size="large" @click="exportData()">
          <Icon type="ios-download-outline"></Icon>导出
        </Button>
        <div style="float: right;">
          <Page :total="page.total" :page-size="page.PageSize" :current="page.current" @on-change="changePage"
            @on-page-size-change="changePageSize" show-sizer show-total></Page>
        </div>
      </div>
    </Card>

    <Modal v-model="itemConfig_Modal" width="60%">
      <p slot="header" style="text-align:center">
        <span>{{titleName.name}}</span>
      </p>
      <div>
        <Form ref="configformValidate" :label-width="120">
          <FormItem v-for="item in configData" :key="item.SerialNo" :label="item.ItmeName+'：'">
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
  import {
    productLine,
    hardType,
    hardItem
  } from '@/api/data'
  export default {
    name: 'purchasing-hard-query',

    data() {
      return {
        searchValue: '',
        ProLine: [], // 产品线
        hardTypeData: [], //大类数据
        bigTypeNo: '', // 默认大类
        smallType: [], // 小类数据
        tag_name: 'default', // 默认标签
        tagCount: [],
        hardItmeList: [], //硬件列表
        ItmeColumns: [], // 硬件列表展示字段

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
        itemConfig_Modal: false, // 硬件配置弹出框
        titleName: {
          // 弹出框展现信息
          // 弹出框熟悉
          id: '', // big small  config item
          name: '', // title
          type: '' // add edit
        },
        page: { //表单分页
          total: 1,
          current: 1,
          PageSize: 10,
          key: '',
        }

      }
    },
    mounted() {
      // 初始化产品线
      productLine.getProLine().then(res => {
        this.ProLine = res

        const Line = this.ProLine[0];
        this.tag_name = Line.DictName;
         this.tagCount.push(Line.DictName);
          this.handleSearch();
      })
      //获取硬件分类
      hardType.getHardType().then(res => {
        this.hardTypeData = res.Data
      })

      this.sethardColumns()

     
    },
    methods: {
      // 硬件大分类选择事件
      getSmall(Option) {
        this.bigType = Option
        this.smallType = []
        this.getsmallType()
      },
      // 大类切换 加载小分类
      getsmallType() {
        const SerialNo = this.bigTypeNo

        const HardType = this.hardTypeData.filter(item => {
          return item.SerialNo === SerialNo
        })

        setTimeout(() => {
          this.smallType = HardType[0].smallType ? HardType[0].smallType : []
        }, 300)
      },
      // 标签点击事件
      tagClick(name) {
        this.tag_name = name.DictName
        this.tagAdd(name.DictName)
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
      getItemData() {
        hardItem
          .getItemDatabySearch({
            PageIndex: this.page.current,
            PageSize: this.page.PageSize,
            QueryKey: this.page.key
          })
          .then(res => {
            console.log(res)
            var data = res.Data
            this.hardItmeList = data.rows.map(item => {
              if (item.ConfigDesc) {

                item.ConfigDesc = JSON.parse(item.ConfigDesc);
              } else {
                item.ConfigDesc = [];

              }
              return item;
            });
            this.page.total = data.total
          })
      },
      handleSearch() {
        if (this.searchValue.length > 0) {
          this.tagAdd(this.searchValue)
        }
        this.page.current = 1;
        this.page.PageSize = 10
        this.page.key = JSON.stringify(this.tagCount);
        this.getItemData();
        console.log(JSON.stringify(this.tagCount))


      },
      sethardColumns() {
        this.ItmeColumns = [        {
            title: '产品线',
            key: 'DictName',
            sortable: true,
            width: 100,
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
                    type: this.hardItmeList[params.index].ConfigDesc.length > 0 ?
                      'ios-eye' :
                      '',
                    size: this.hardItmeList[params.index].ConfigDesc.length > 0 ?
                      '24' :
                      '24'
                  },
                  on: {
                    click: () => {
                      this.onSelect.row = params.row
                      this.getItemConfig()
                    }
                  }
                }),
                h(
                  'div', {
                    slot: 'content'
                  },
                  this.hardItmeList[params.index].ConfigDesc.length > 0 ?
                  '点击查看/编辑' :
                  '点击设置'
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
              const text = params.row.WarrantyPeriod ?
                params.row.WarrantyPeriod + '/年' :
                ''
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
                    type: this.hardItmeList[params.index].AttachFiles ?
                      'ios-eye' :
                      '',
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
                  this.hardItmeList[params.index].AttachFiles ?
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
              const text = params.row.DeliveryDay ?
                params.row.DeliveryDay + '/工作日' :
                ''
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
      // 查看硬件配置
      getItemConfig() {
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
      show(index) {
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
      //分页事件
      changePage(value) {
        this.page.current = value
        this.getItemData()
      },
      changePageSize(value) {
        this.page.PageSize = value
        this.getItemData()
      },
      exportData() {
        this.$refs.tables.exportCsv({
          filename: `硬件列表-${(new Date()).valueOf()}.csv`,
          columns: this.hardColumns.filter((col, index) => index != 5),
          data: this.hardList.filter((data, index) => index != 5)
        })

      }
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
