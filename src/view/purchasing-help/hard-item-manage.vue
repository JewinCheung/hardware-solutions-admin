<template>
  <div class="hard-iem-manage">
    <Card>
      <div class="search-con search-con-top">
        <Input clearable search size="large" v-model="searchValue" placeholder="输入关键字搜索" style="width:80%" />
        <Button class="search-btn" type="primary" @click="handleSearch">
          <Icon type="search" />&nbsp;&nbsp;搜索</Button>

      </div>
      <div class="tag-nav-line" style="height:30px">
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
      <div class="tools">
        <ButtonGroup>
          <Button type="info" icon="md-add" @click="addItem">新增</Button>
          <Button type="info" icon="md-create" @click="editItem">编辑</Button>
          <Button type="info" icon="md-checkmark">启用</Button>
          <Button type="info" icon="md-close">停用</Button>
        </ButtonGroup>
      </div>
      <Table :data="hardList" :columns="hardColumns" stripe :highlight-row="true" ref="tables" height="520"
        @on-row-click="onRowClick" @on-row-dblclick="onRowDblclick"></Table>
      <div style="margin: 10px;overflow: hidden">
        <div style="float: right;">
          <Page :total="page.total" :page-size="page.PageSize" :current="page.current" @on-change="changePage"
            @on-page-size-change="changePageSize" show-sizer show-total></Page>
        </div>
      </div>
    </Card>

    <Modal v-model="item_Modal" width="80%">
      <p slot="header" style="text-align:center">
        <span>{{titleName.name}}</span>
      </p>
      <div>
        <Form ref="itemForm" :model="itemForm" :rules="ruleValidate_Item" :label-width="100">
          <FormItem label="硬件大类:">
            <Row>
              <Col span="6">
              <FormItem prop="MatTypeNo">
                <Select v-model="itemForm.MatTypeNo" style="width:160px" @on-change="getSmallToForm">
                  <Option v-for="item in hardTypeData" :value="item.MatTypeNo" :key="item.MatTypeNo">{{
                    item.MatTypeName }}</Option>
                </Select>
              </FormItem>
              </Col>
              <Col span="9">
              <FormItem label="硬件小类:" prop="SubItemNo">
                <Select ref="SubItem" v-model="itemForm.SubItemNo" style="width:160px" clearable>
                  <Option v-for="item in smallTypeF" :value="item.MatTypeNo" :key="item.MatTypeNo">{{ item.MatTypeName
                    }}</Option>
                </Select>
              </FormItem>
              </Col>
            </Row>
          </FormItem>
          <FormItem label="硬件品牌:">
            <Row>
              <Col span="6">
              <FormItem prop="Brand">
                <Input v-model="itemForm.Brand" style="width:160px" />
              </FormItem>
              </Col>
              <Col span="9">
              <FormItem label="硬件型号:" prop="ItemModel">
                <Input v-model="itemForm.ItemModel" style="width:160px" />
              </FormItem>
              </Col>
              <Col span="9">
              <FormItem label="是否停用:" prop="IsStop">
                <Select v-model="itemForm.IsStop" style="width:160px">
                  <Option value="0">否</Option>
                  <Option value="1">是</Option>
                </Select>
              </FormItem>
              </Col>
            </Row>
          </FormItem>

          <FormItem label="质保期限:">
            <Row>
              <Col span="6">
              <FormItem prop="WarrantyPeriod">
                <InputNumber :min="1" v-model="itemForm.WarrantyPeriod" style="width:160px" />/年
              </FormItem>
              </Col>
              <Col span="9">
              <FormItem label="质保类型:" prop="warrantyType">
                <Select v-model="itemForm.warrantyType" style="width:160px">
                  <Option value="上门">上门</Option>
                  <Option value="送修">送修</Option>
                </Select>
              </FormItem>
              </Col>
              <Col span="9">
              <FormItem label="延保费用:" prop="WarrantyCost">
                <InputNumber v-model="itemForm.WarrantyCost" :formatter="value => `${value}`.replace(/B(?=(d{3})+(?!d))/g, ',')"
                  :parser="value => value.replace(/$s?|(,*)/g, '')" style="width:160px" />
              </FormItem>
              </Col>
            </Row>
          </FormItem>
          <FormItem label="采购价格:">
            <Row>
              <Col span="6">
              <FormItem prop="PurchasePrice">
                <InputNumber v-model="itemForm.PurchasePrice" :formatter="value => `${value}`.replace(/B(?=(d{3})+(?!d))/g, ',')"
                  :parser="value => value.replace(/$s?|(,*)/g, '')" style="width:160px" />
              </FormItem>
              </Col>
              <Col span="9">
              <FormItem label="建议报价:" prop="Offer">
                <InputNumber v-model="itemForm.Offer" :formatter="value => `${value}`.replace(/B(?=(d{3})+(?!d))/g, ',')"
                  :parser="value => value.replace(/$s?|(,*)/g, '')" style="width:160px" />
              </FormItem>
              </Col>
              <Col span="9">
              <FormItem label="节能环保:" prop="IsEnergy">
                <Select v-model="itemForm.IsEnergy" style="width:160px">
                  <Option value=0>否</Option>
                  <Option value=1>是</Option>
                </Select>
              </FormItem>
              </Col>
            </Row>
          </FormItem>
          <FormItem label="硬件图片:">
            <div class="demo-upload-list" v-for="(item,index) in uploadList" :key="index">
              <template v-if="item.status === 'finished'">
                <img :src="item.url">
                <div class="demo-upload-list-cover">
                  <Icon type="ios-eye-outline" @click.native="handleView(item)"></Icon>
                  <Icon type="ios-trash-outline" @click.native="handleRemove(item)"></Icon>
                </div>
              </template>
              <template v-else>
                <Progress v-if="item.showProgress" :percent="item.percentage" hide-info></Progress>
              </template>
            </div>
            <Upload ref="upload" :show-upload-list="false" :on-success="handleSuccess" :default-file-list="defaultList"
              :on-format-error="handleFormatError" :on-exceeded-size="handleMaxSize" :before-upload="handleBeforeUpload"
              :format="['jpg','jpeg','png']" :max-size="2048" multiple type="drag" action="http://localhost:9001/Api/UploadFile/UploadFile"
              style="display: inline-block;width:58px;">
              <div style="width: 58px;height:58px;line-height: 58px;">
                <Icon type="ios-camera" size="20"></Icon>
              </div>
            </Upload>
          </FormItem>
        </Form>

      </div>
      <div slot="footer">
        <Button type="primary" @click="handleItemSubmit('itemForm')" :loading="item_loading">保存</Button>
        <Button @click="item_Modal=false" style="margin-left: 8px">关闭</Button>
      </div>
    </Modal>

    <Modal title="图片预览" v-model="visible">
      <img :src="imgUrl" v-if="visible" style="width: 100%">
    </Modal>
  </div>
</template>

<script>
import {
  hardType,
  hardItem
} from '@/api/data'
import {
  resData
} from '@/libs/tools'
export default {
  name: 'hard-item-manage',
  data () {
    return {
      HardType: [], // 硬件分类

      hardColumns: [],

      hardList: [],
      item_loading: false,
      searchValue: '',
      bigTypeNo: '', // 模式大类
      hardTypeData: [],
      smallType: [], // 硬件小类
      tag_name: 'default', // 默认标签
      tagCount: [],
      onSelect: {
        index: 0,
        row: null
      },
      titleName: {
        // 弹出框熟悉
        id: '', // big small  config item
        name: '', // title
        type: '' // add edit
      },
      smallTypeF: [],
      item_Modal: false,
      itemForm: {
        IsStop: '0',
        IsEnergy: '1',
        WarrantyPeriod: 1,
        WarrantyCost: 1,
        PurchasePrice: 1,
        Offer: 1
      },
      uploadList: [],
      defaultList: [],
      imgUrl: '',
      visible: false,
      ruleValidate_Item: {
        // 类型表单验证
        MatTypeNo: [{
          required: true,
          message: '不能为空'
        }],
        SubItemNo: [{
          required: true,
          message: '不能为空'
        }],
        Brand: [{
          required: true,
          message: '不能为空'
        }],
        ItemModel: [{
          required: true,
          message: '不能为空'
        }]
      },
      page: {
        total: 1,
        current: 1,
        PageSize: 10
      }
    }
  },
  mounted () {
    // 初始化硬件分类

    // 硬件列表
    this.sethardColumns()
    this.handleSearch()

    this.getHardTypeData()
    this.getItemData()
  },
  methods: {
    getItemData () {
      hardItem
        .getItemData({
          PageIndex: this.page.current,
          PageSize: this.page.PageSize
        })
        .then(res => {
          console.log(res)
          var data = res.Data
          this.hardList = data.rows
          this.page.total = data.total
        })
    },

    getHardTypeData () {
      hardType.getHardType().then(res => {
        this.hardTypeData = res.Data
      })
    },

    // 切换硬件小分类
    getsmallType () {
      const SerialNo = this.bigTypeNo

      const HardType = this.hardTypeData.filter(item => {
        return item.SerialNo === SerialNo
      })

      setTimeout(() => {
        this.smallType = HardType[0].smallType ? HardType[0].smallType : []
      }, 300)
    },
    // 标签点击事件
    tagClick (name) {
      this.tag_name = name
      this.tagAdd(name)
    },
    // 添加标签
    tagAdd (name) {
      const index = this.tagCount.indexOf(name)
      if (index < 0) {
        this.tagCount.push(name)
      }
    },
    // 关闭标签
    tagClose (name) {
      const index = this.tagCount.indexOf(name)
      this.tagCount.splice(index, 1)
    },
    // 硬件大分类选择事件
    getSmall (Option) {
      this.bigType = Option
      this.smallType = []
      this.getsmallType()
    },
    getSmallToForm () {
      const SerialNo = this.itemForm.MatTypeNo

      const HardType = this.hardTypeData.filter(item => {
        return item.MatTypeNo === SerialNo
      })
      this.smallTypeF = HardType.length > 0 ? HardType[0].smallType : []
      this.$refs.SubItem.clearSingleSelect()
      // this.itemForm.SubItemNo=HardType[0].smallType[0].MatTypeNo;
    },
    addItem () {
      this.$refs.itemForm.resetFields() // 重置表单
      this.item_Modal = true
      this.$refs.upload.clearFiles()
      this.uploadList = this.$refs.upload.fileList

      this.titleName.name = '新增-硬件信息'
    },

    editItem () {
      if (!this.onSelect.row) {
        this.$Message.error('请选择行！')
        return
      }
      this.$refs.itemForm.resetFields() // 重置表单
      var row = this.onSelect.row
      this.itemForm = this.onSelect.row
      this.itemForm.IsStop = '' + row.IsStop + ''
      this.itemForm.IsEnergy = '' + row.IsEnergy + ''

      this.$refs.upload.clearFiles()

      if (row.AttachFiles) {
        var AttachFiles = JSON.parse(row.AttachFiles)
        AttachFiles = AttachFiles.map(item => {
          return {
            name: item.url,
            url: item.url,
            status: 'finished'
          }
        })
        this.$refs.upload.fileList = AttachFiles
        this.uploadList = this.$refs.upload.fileList
      }
      this.item_Modal = true
    },
    onRowClick (row, index) {
      this.onSelect.row = row
    },
    onRowDblclick (row, index) {
      this.onSelect.row = row
      this.editItem()
    },
    handleSuccess (res, file, fileList) {
      console.log(res)
      file.url = res.Data
      file.name = res.Data
      console.log(this.uploadList)
    },
    handleFormatError (file) {
      this.$Notice.warning({
        title: '文件格式不正确',
        desc: '图片' + file.name + ' 格式不正确，请选择jpg或png格式.'
      })
    },
    handleMaxSize (file) {
      this.$Notice.warning({
        title: '超过文件大小限制',
        desc: '图片：  ' + file.name + ' 太大了，不能超过2M.'
      })
    },
    handleBeforeUpload () {
      const check = this.uploadList.length < 5
      if (!check) {
        this.$Notice.warning({
          title: '最多可以上传5张照片.'
        })
      }
      return check
    },

    handleView (item) {
      this.imgUrl = item.url
      this.visible = true
    },
    handleRemove (file) {
      const fileList = this.$refs.upload.fileList
      this.$refs.upload.fileList.splice(fileList.indexOf(file), 1)
      console.log(this.uploadList)
    },

    handleItemSubmit (name) {
      var postData = this.itemForm
      this.$refs[name].validate(valid => {
        if (valid) {
          this.item_loading = true
          if (this.uploadList.length > 0) {
            var fileData = this.uploadList.map(item => {
              return {
                url: item.url
              }
            })
            postData['AttachFiles'] = JSON.stringify(fileData)
          }
          hardItem.saveItem(postData).then(res => {
            this.item_loading = false
            resData(
              res,
              res => {
                this.item_Modal = false
                this.$Message.success(res.Msg)
                this.getItemData()
              },
              res => {
                this.$Message.error(res.Msg)
              }
            )
          })
        }
      })
    },
    changePage (value) {
      this.page.current = value
      this.getItemData()
    },
    changePageSize (value) {
      this.page.PageSize = value
      this.getItemData()
    },
    sethardColumns () {
      this.hardColumns = [{
        title: '大类',
        key: 'MatTypeName',
        sortable: true
      },
      {
        title: '子类',
        key: 'SubType',
        sortable: true
      },
      {
        title: '品牌',
        key: 'Brand',
        sortable: true
      },
      {
        title: '型号',
        key: 'ItemModel',
        tooltip: true
      },
        // {
        //   title: "参数",
        //   key: "hardParams",
        //   render: (h, params) => {
        //     return h(
        //       "tooltip",
        //       {
        //         props: {
        //           placement: "top"
        //         }
        //       },
        //       [
        //         h("Tag", "···"),
        //         h(
        //           "div",
        //           {
        //             slot: "content"
        //           },
        //           [
        //             h(
        //               "ul",
        //               this.hardList[params.index].hardParams.map(item => {
        //                 return h(
        //                   "li",
        //                   {
        //                     style: {
        //                       textAlign: "left",
        //                       padding: "4px",
        //                       "list-style-type": "none"
        //                     }
        //                   },
        //                   item.dictType + "：" + item.dictName
        //                 );
        //               })
        //             )
        //           ]
        //         )
        //       ]
        //     );
        //   }
        // },
      {
        title: '质保',
        key: 'WarrantyPeriod',
        width: 60,
        align: 'center',
        render: (h, params) => {
          const text = params.row.WarrantyPeriod
            ? params.row.WarrantyPeriod + '/年'
            : ''
          return h('div', text)
        }
      },
      {
        title: '质保',
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
                type: this.hardList[params.index].AttachFiles
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
              'div', {
                slot: 'content'
              },
              this.hardList[params.index].AttachFiles
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
        title: '状态',
        key: 'IsStop',
        width: 60,
        align: 'center',
        render: (h, params) => {
          const row = params.row
          const text = row.IsStop === 1 ? '停用' : '启用'
          return h('div', text)
        }
      },
      {
        title: '更新日期',
        key: 'LastTime',
        width: 102,
        sortable: true
      }
      ]
    },
    show (index) {
      if (this.hardList[index].AttachFiles) {
        var AttachFiles = JSON.parse(this.hardList[index].AttachFiles)

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
    handleSearch () {
      if (this.searchValue.length > 0) {
        this.tagAdd(this.searchValue)
      }
      // 硬件列表
      // getHardList().then(res => {
      //   this.hardList = res.data
      //   if (this.tagCount.length > 0) {
      //     this.hardList = this.hardList.filter(item => {
      //       var isOk = false
      //       this.tagCount.map(i => {
      //         if (
      //           item.bigType.indexOf(i) > -1 ||
      //           item.smallType.indexOf(i) > -1
      //         ) {
      //           return (isOk = true)
      //         }
      //       })
      //       if (isOk) {
      //         return item
      //       }
      //     })
      //   }
      // })
    }
  }
}

</script>

<style lang="less">
  .demo-upload-list {
    display: inline-block;
    width: 60px;
    height: 60px;
    text-align: center;
    line-height: 60px;
    border: 1px solid transparent;
    border-radius: 4px;
    overflow: hidden;
    background: #fff;
    position: relative;
    box-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);
    margin-right: 4px;
  }

  .demo-upload-list img {
    width: 100%;
    height: 100%;
  }

  .demo-upload-list-cover {
    display: none;
    position: absolute;
    top: 0;
    bottom: 0;
    left: 0;
    right: 0;
    background: rgba(0, 0, 0, 0.6);
  }

  .demo-upload-list:hover .demo-upload-list-cover {
    display: block;
  }

  .demo-upload-list-cover i {
    color: #fff;
    font-size: 20px;
    cursor: pointer;
    margin: 0 2px;
  }

  .hard-iem-manage {
    .tools {
      margin: 0 0 10px 0;

      // Button {
      //   margin: 5px;
      // }
    }

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
        margin: 0 5px 0 5px;
        border: 1;
      }

      .ivu-tag-primary {
        border: 1 !important;
      }
    }
  }

</style>
