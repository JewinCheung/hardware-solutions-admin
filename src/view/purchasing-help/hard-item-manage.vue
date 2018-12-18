<template>
  <div class="hard-iem-manage">
    <Card>
      <div class="search-con search-con-top">
        <Input
          clearable
          search
          size="large"
          v-model="searchValue"
          placeholder="输入关键字搜索"
          style="width:80%"
        />
        <Button class="search-btn" type="primary" @click="handleSearch">
          <Icon type="search"/>&nbsp;&nbsp;搜索
        </Button>
      </div>

      <div class="tag-nav-line" style="height:30px">硬件分类：
        <Select v-model="bigTypeNo" style="width:160px" @on-change="getSmall">
          <Option
            v-for="item in hardTypeData"
            :value="item.SerialNo"
            :key="item.SerialNo"
          >{{ item.MatTypeName }}</Option>
        </Select>
        <Tag
          v-for="item in smallType"
          :key="item.SerialNo"
          :name="item.MatTypeName"
          color="default"
          @click.native="tagAdd(item.MatTypeName)"
        >
          {{
          item.MatTypeName }}
        </Tag>
      </div>
      <div style="height:22px;margin: 5px 0 5px 0;">
        <Tag
          closable
          v-for="item in tagCount"
          :key="item"
          :name="item.title"
          color="warning"
          @on-close="tagClose(item)"
        >
          {{
          item}}
        </Tag>
      </div>
      <div class="tools">
        <ButtonGroup>
          <Button type="info" icon="md-add" @click="addItem">新增</Button>
          <Button type="info" icon="md-create" @click="editItem">编辑</Button>
          <Button type="info" icon="md-settings" @click="setItem">配置</Button>
          <!-- <Button type="info" icon="md-checkmark">启用</Button>
          <Button type="info" icon="md-close">停用</Button>-->
        </ButtonGroup>
      </div>
      <Table
        :data="hardList"
        :columns="hardColumns"
        stripe
        :highlight-row="true"
        ref="tables"
        height="480"
        width="100%"
        @on-row-click="onRowClick"
        @on-row-dblclick="onRowDblclick"
      ></Table>
      <div style="margin: 10px;overflow: hidden">
        <div style="float: right;">
          <Page
            :total="page.total"
            :page-size="page.PageSize"
            :current="page.current"
            @on-change="changePage"
            @on-page-size-change="changePageSize"
            show-sizer
            show-total
          ></Page>
        </div>
      </div>
    </Card>

    <Modal v-model="item_Modal" width="80%">
      <p slot="header" style="text-align:center">
        <span>{{titleName.name}}</span>
      </p>
      <div>
        <Form ref="itemsForm" :model="itemForm" :rules="ruleValidate_Item" :label-width="100">
          <FormItem label="硬件大类:">
            <Row>
              <Col span="6">
                <FormItem prop="MatTypeNo">
                  <Select
                    v-model="itemForm.MatTypeNo"
                    style="width:160px"
                    @on-change="getSmallToForm"
                  >
                    <Option
                      v-for="item in hardTypeData"
                      :value="item.MatTypeNo"
                      :key="item.MatTypeNo"
                    >
                      {{
                      item.MatTypeName }}
                    </Option>
                  </Select>
                </FormItem>
              </Col>
              <Col span="9">
                <FormItem label="硬件小类:" prop="SubItemNo">
                  <Select ref="SubItem" v-model="itemForm.SubItemNo" style="width:160px" clearable>
                    <Option
                      v-for="item in smallTypeF"
                      :value="item.MatTypeNo"
                      :key="item.MatTypeNo"
                    >
                      {{ item.MatTypeName
                      }}
                    </Option>
                  </Select>
                </FormItem>
              </Col>
              <Col span="9">
                <FormItem label="采购周期:" prop="DeliveryDay">
                  <InputNumber :min="1" v-model="itemForm.DeliveryDay" style="width:160px"/>/工作日
                </FormItem>
              </Col>
            </Row>
          </FormItem>
          <FormItem label="硬件品牌:">
            <Row>
              <Col span="6">
                <FormItem prop="Brand">
                  <Input v-model="itemForm.Brand" style="width:160px"/>
                </FormItem>
              </Col>
              <Col span="9">
                <FormItem label="硬件型号:" prop="ItemModel">
                  <Input v-model="itemForm.ItemModel" style="width:160px"/>
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
                  <InputNumber :min="1" v-model="itemForm.WarrantyPeriod" style="width:160px"/>/年
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
                  <InputNumber
                    v-model="itemForm.WarrantyCost"
                    :formatter="value => `${value}`.replace(/B(?=(d{3})+(?!d))/g, ',')"
                    :parser="value => value.replace(/$s?|(,*)/g, '')"
                    style="width:160px"
                  />
                </FormItem>
              </Col>
            </Row>
          </FormItem>
          <FormItem label="采购价格:">
            <Row>
              <Col span="6">
                <FormItem prop="PurchasePrice">
                  <InputNumber
                    v-model="itemForm.PurchasePrice"
                    :formatter="value => `${value}`.replace(/B(?=(d{3})+(?!d))/g, ',')"
                    :parser="value => value.replace(/$s?|(,*)/g, '')"
                    style="width:160px"
                  />
                </FormItem>
              </Col>
              <Col span="9">
                <FormItem label="建议报价:" prop="Offer">
                  <InputNumber
                    v-model="itemForm.Offer"
                    :formatter="value => `${value}`.replace(/B(?=(d{3})+(?!d))/g, ',')"
                    :parser="value => value.replace(/$s?|(,*)/g, '')"
                    style="width:160px"
                  />
                </FormItem>
              </Col>
              <Col span="9">
                <FormItem label="节能环保:" prop="IsEnergy">
                  <Select v-model="itemForm.IsEnergy" style="width:160px">
                    <Option value="0">否</Option>
                    <Option value="1">是</Option>
                  </Select>
                </FormItem>
              </Col>
            </Row>
          </FormItem>
          <FormItem label="硬件图片:">
            <div class="demo-upload-list" v-for="item in uploadList" :key="item.url">
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
            <Upload
              ref="upload"
              :show-upload-list="false"
              :on-success="handleSuccess"
              :default-file-list="defaultList"
              :on-format-error="handleFormatError"
              :on-exceeded-size="handleMaxSize"
              :before-upload="handleBeforeUpload"
              :format="['jpg','jpeg','png']"
              :max-size="2048"
              multiple
              type="drag"
              :action="actionUrl"
              style="display: inline-block;width:58px;"
            >
              <div style="width: 58px;height:58px;line-height: 58px;">
                <Icon type="ios-camera" size="20"></Icon>
              </div>
            </Upload>
          </FormItem>
        </Form>
      </div>
      <div slot="footer">
        <Button type="primary" @click="handleItemSubmit('itemsForm')" :loading="item_loading">保存</Button>
        <Button @click="item_Modal=false" style="margin-left: 8px">关闭</Button>
      </div>
    </Modal>

    <Modal title="图片预览" v-model="visible">
      <img :src="imgUrl" v-if="visible" style="width: 100%">
    </Modal>

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
            <RadioGroup v-model="item.onSelect">
              <Radio
                v-for="SubItem in item.ConfigItme"
                :key="SubItem.SerialNo"
                :label="SubItem.SerialNo"
              >{{SubItem.ItmeName}}</Radio>
            </RadioGroup>
          </FormItem>
        </Form>
      </div>
      <div slot="footer">
        <Button
          type="primary"
          @click="handleConfigSubmit('configformValidate')"
          :loading="item_loading"
        >保存</Button>
        <Button
          type="warning"
          @click="handleReset('configformValidate')"
          style="margin-left: 8px"
        >重置</Button>
        <Button @click="itemConfig_Modal=false" style="margin-left: 8px">关闭</Button>
      </div>
    </Modal>
  </div>
</template>

<script>
import { hardType, hardItem } from '@/api/data'
import { resData } from '@/libs/tools'
export default {
  name: 'hard-item-manage',
  data () {
    return {
      default: '',
      animal: '',
      configData: [
        {
          SerialNo: '004BD9FB-C892-4390-941C-59527F01sdfE',
          ItmeName: 'CPU型号',
          onSelect: '',
          ConfigItme: [
            {
              SerialNo: '004BD9FB-C892-4390-941C-5952sdfdsf323E',
              ItmeName: 'Intel Core i5-6500'
            },
            {
              SerialNo: '004BD9FB-C892-4390-941C-5952sdsdfsdf',
              ItmeName: 'Intel Core i3-3000'
            }
          ]
        },
        {
          SerialNo: '004BD9FB-C892-4390-941C-59527F012323E',
          ItmeName: 'CPU核数',
          onSelect: '004BD9FB-C892-4390-941C-5952sdfdsf12',
          ConfigItme: [
            {
              SerialNo: '004BD9FB-C892-4390-941C-5952sdfdsf12',
              ItmeName: '2.0hz'
            },
            {
              SerialNo: '004BD9FB-C892-4390-941C-5952sdsdf33',
              ItmeName: '3.0hz'
            }
          ]
        }
      ],
      configForm: [
        {
          ParentNo: '004BD9FB-C892-4390-941C-59527F01sdfE',
          ItmeName: 'CPU型号',
          SerialNo: '004BD9FB-C892-4390-941C-5952sdfdsf323E',
          SubItmeName: 'Intel Core i5-6500'
        },
        {
          ParentNo: '004BD9FB-C892-4390-941C-59527F012323E',
          ItmeName: 'CPU核数',
          SerialNo: '004BD9FB-C892-4390-941C-5952sdsdf33',
          SubItmeName: '3.0Hz'
        }
      ],

      hardList: [], // 硬件列表
      hardTypeData: [], // 大类数据
      hardColumns: [], // 硬件列表展示字段
      actionUrl: 'http://e.mdsd.cn:9000/Api/UploadFile/UploadFile',
      // actionUrl: "http://localhost:9000/Api/UploadFile/UploadFile", //上传图片API
      item_loading: false, // 按钮禁用
      searchValue: '', //
      bigTypeNo: '', // 默认大类
      smallType: [], // 小类数据
      tag_name: 'default', // 默认标签
      tagCount: [], // 所选标签组
      onSelect: {
        // 当前选中行
        index: 0,
        row: null
      },
      titleName: {
        // 弹出框展现信息
        // 弹出框熟悉
        id: '', // big small  config item
        name: '', // title
        type: '' // add edit
      },
      smallTypeF: [], // 硬件编辑框小类数据
      item_Modal: false, // 硬件详情编辑框
      itemConfig_Modal: false, // 硬件配置弹出框
      itemForm: {
        // 表单默认值
        IsStop: '0',
        IsEnergy: '1',
        WarrantyPeriod: 1,
        WarrantyCost: 1,
        PurchasePrice: 1,
        Offer: 1,
        DeliveryDay: 1
      },
      uploadList: [], // 上传-已上传图片组
      defaultList: [], // 默认已上传组
      imgUrl: '', // 图片预览地址
      visible: false, // 开启 关闭
      ruleValidate_Item: {
        // 类型表单验证
        MatTypeNo: [
          {
            required: true,
            message: '不能为空'
          }
        ],
        SubItemNo: [
          {
            required: true,
            message: '不能为空'
          }
        ],
        Brand: [
          {
            required: true,
            message: '不能为空'
          }
        ],
        ItemModel: [
          {
            required: true,
            message: '不能为空'
          }
        ]
      },
      page: {
        // 表单分页
        total: 1,
        current: 1,
        PageSize: 10,
        key: ''
      }
    }
  },
  mounted () {
    // 初始化硬件分类

    // 硬件列表
    this.sethardColumns()
    this.handleSearch()

    this.getHardTypeData()
    // this.getItemData()
  },
  methods: {
    // 获取硬件配置
    getConfigData (SerialNo) {
      this.configData = []
      hardType.getTypeConfig(SerialNo).then(res => {
        this.configData = res.Data
        if (res.Data.length === 0) {
          this.$Message.warning('无硬件配置')
        }
      })
    },
    // 加载硬件信息
    getItemData () {
      hardItem
        .getItemData({
          PageIndex: this.page.current,
          PageSize: this.page.PageSize,
          QueryKey: this.page.key
        })
        .then(res => {
          console.log(res)
          var data = res.Data
          this.hardList = data.rows.map(item => {
            if (item.ConfigDesc) {
              item.ConfigDesc = JSON.parse(item.ConfigDesc)
            } else {
              item.ConfigDesc = []
            }
            return item
          })
          this.page.total = data.total
        })
    },
    // 加载大类
    getHardTypeData () {
      hardType.getHardType().then(res => {
        this.hardTypeData = res.Data
      })
    },

    // 大类切换 加载小分类
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
    // 大类切换 加载小分类 form
    getSmallToForm () {
      const SerialNo = this.itemForm.MatTypeNo
      const HardType = this.hardTypeData.filter(item => {
        return item.MatTypeNo === SerialNo
      })
      this.smallTypeF = HardType.length > 0 ? HardType[0].smallType : []
    },
    // 添加硬件详情
    addItem () {
      this.$refs.tables.clearCurrentRow() // 清除 表格选中高亮
      this.$refs.itemsForm.resetFields() // 重置表单
      this.item_Modal = true
      this.$refs.upload.clearFiles() // 清理上传列表
      this.uploadList = this.$refs.upload.fileList // 初始化 上传列表  转化对象格式{  name: item.url,url: item.url,status: 'finished'....}
      this.onSelect.row = {
        // 重置表单后 会清空默认值   临时处理：重新赋默认值
        IsStop: '0',
        IsEnergy: '1',
        WarrantyPeriod: 1,
        WarrantyCost: 1,
        PurchasePrice: 1,
        Offer: 1,
        DeliveryDay: 1
      } // 清除表格索引
      this.titleName.name = '新增-硬件信息' // titleName
      this.itemForm = this.onSelect.row // 表单赋值
      this.onSelect.row = null
    },
    // 编辑详情
    editItem () {
      if (!this.onSelect.row) {
        this.$Message.error('请选择行！')
        return
      }
      var row = this.onSelect.row
      this.itemForm = this.onSelect.row
      // 表单-下拉列表value赋值 居然不支持int类型 无奈临时处理 随意转化成string
      this.itemForm.IsStop = '' + row.IsStop + ''
      this.itemForm.IsEnergy = '' + row.IsEnergy + ''
      this.getSmallToForm() // 硬件小类数据源赋值
      this.$refs.upload.clearFiles() // 清空文件列表

      // 如果存在文件
      if (row.AttachFiles) {
        var AttachFiles = JSON.parse(row.AttachFiles)
        AttachFiles = AttachFiles.map(item => {
          return {
            name: item.url,
            url: item.url,
            status: 'finished'
          }
        })
        // 文件列表加载默认文件
        this.$refs.upload.fileList = AttachFiles
      }
      this.uploadList = this.$refs.upload.fileList // 转换已上传列表
      this.titleName.name = '编辑-硬件信息'
      this.item_Modal = true
      // 清空当前选中项， 由于数据双向绑定  不清空的话   再次点击编辑会造成表单数据丢失
      // this.onSelect.row = null;
      // this.$refs.tables.clearCurrentRow() // 清除 表格选中高亮
    },
    // 保存硬件信息
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
    // 硬件配置
    setItem () {
      if (!this.onSelect.row) {
        this.$Message.error('请选择行！')
        return
      }
      let rows = this.onSelect.row

      this.configData = []
      let SerialNo = rows.TypeNo
      hardType.getTypeConfig(SerialNo).then(res => {
        this.configData = res.Data
        if (res.Data.length === 0) {
          this.$Message.warning('无硬件配置')
        }

        this.titleName.name =
          '[' + rows.SubType + '-' + rows.ItemModel + ']配置硬件信息'
        let ConfigDesc = this.onSelect.row.ConfigDesc

        if (ConfigDesc) {
          // let ConfigData = JSON.parse(rows.ConfigDesc);
          let NewData = []
          NewData = this.configData.map(item => {
            let onItem = ConfigDesc.filter(
              sub => sub.ParentNo === item.SerialNo
            )
            let onSelect = ''
            if (onItem.length > 0) {
              onSelect = onItem[0].SerialNo
            }
            item['onSelect'] = onSelect
            return item
          })

          this.configData = NewData
          console.log(this.configData)
        }

        this.itemConfig_Modal = true
      })
    },
    // 保存硬件配置
    handleConfigSubmit (name) {
      let configData = this.configData.filter(item => item.onSelect)

      let data = configData.map(item => {
        let onSelect = item.onSelect
        let itemList = item.ConfigItme

        let onItem = itemList.filter(i => {
          return i.SerialNo === onSelect
        })

        return {
          ParentNo: item.SerialNo,
          ItmeName: item.ItmeName,
          SerialNo: item.onSelect,
          SubItmeName: onItem[0].ItmeName
        }
      })
      console.log(JSON.stringify(data))
      let postData = {
        SerialNo: this.onSelect.row.SerialNo,
        ConfigDesc: JSON.stringify(data)
      }
      this.item_loading = true
      hardItem.setConfig(postData).then(res => {
        this.item_loading = false
        resData(
          res,
          res => {
            this.itemConfig_Modal = false
            this.$Message.success(res.Msg)
            this.getItemData()
          },
          res => {
            this.$Message.error(res.Msg)
          }
        )
      })
    },
    handleReset (name) {
      let rows = this.onSelect.row
      this.configData = []
      let SerialNo = rows.TypeNo
      this.getConfigData(SerialNo)
      // this.$refs[name].resetFields();
    },
    // 列表单击事件
    onRowClick (row, index) {
      this.onSelect.row = row
      // let SerialNo = row.TypeNo
      // this.getConfigData(SerialNo)
      // console.log(this.onSelect.row)
    },
    // 双击
    onRowDblclick (row, index) {
      this.onSelect.row = row
      this.editItem()
    },
    // 上传成功后处理
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
    // 分页事件
    changePage (value) {
      this.page.current = value
      this.getItemData()
    },
    changePageSize (value) {
      this.page.PageSize = value
      this.getItemData()
    },
    sethardColumns () {
      this.hardColumns = [
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
                    this.hardList[params.index].ConfigDesc.length > 0
                      ? 'ios-eye'
                      : 'ios-settings',
                  size:
                    this.hardList[params.index].ConfigDesc.length > 0
                      ? '24'
                      : '24'
                },
                on: {
                  click: () => {
                    this.onSelect.row = params.row
                    this.setItem()
                  }
                }
              }),
              h(
                'div',
                {
                  slot: 'content'
                },
                this.hardList[params.index].ConfigDesc.length > 0
                  ? '点击查看/编辑'
                  : '点击设置'
              )
              // h(
              //   "div", {
              //     slot: "content"
              //   },
              //   [
              //     h(
              //       "ul", {
              //         id: params.row.SerialNo

              //       },
              //       this.hardList[params.index].ConfigDesc.map(item => {
              //         return h(
              //           "li", {
              //             style: {
              //               textAlign: "left",
              //               padding: "4px",
              //               "list-style-type": "none"
              //             },
              //           },
              //           item.ItmeName + "：" + item.SubItmeName
              //         );
              //       })
              //     )
              //   ]
              // )
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
                'div',
                {
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
          minWidth: 110,
          sortable: true
        }
      ]
    },
    copyText (params) {},
    show (index) {
      if (this.hardList[index].AttachFiles) {
        var AttachFiles = JSON.parse(this.hardList[index].AttachFiles)
        var clientHeight = document.body.clientHeight * 0.7 + 'px'
        var newcss = {
          height: clientHeight,
          overflow: 'auto',
          display: 'flex',
          'justify-content': 'center',
          'align-items': 'Center'
        }
        if (AttachFiles.length > 2) {
          newcss = {
            height: clientHeight,
            overflow: 'auto'
          }
        }

        this.$Modal.info({
          scrollable: true,
          render: h => {
            return h(
              'div',
              {
                style: newcss
              },
              [
                h(
                  'div',
                  {
                    style: {
                      height: 'auto'
                    }
                  },
                  JSON.parse(this.hardList[index].AttachFiles).map(item => {
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
              ]
            )
          }
        })
      }
    },
    handleSearch () {
      if (this.searchValue.length > 0) {
        this.tagAdd(this.searchValue)
      }
      // page: { //表单分页
      //   total: 1,
      //   current: 1,
      //   PageSize: 10
      // }
      this.page.current = 1
      this.page.PageSize = 10
      this.page.key = JSON.stringify(this.tagCount)
      this.getItemData()
      console.log(JSON.stringify(this.tagCount))
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
