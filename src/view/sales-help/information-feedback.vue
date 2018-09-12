<template>
  <div class="sales-information-feedback">
    <Card>
      <div class="tools">
        <Button type="primary" @click="addInfo">新增</Button>
        <Button type="primary" @click="editInfo">编辑</Button>
      </div>
      <Table :data="infoData" :columns="infoColumns" stripe ref="tables" :highlight-row="true" @on-row-click="onRowClick"
        @on-row-dblclick="onRowDblclick"></Table>
      <div style="margin: 10px;overflow: hidden">
        <div style="float: right;">
          <Page :total="1" :current="1"></Page>
        </div>
      </div>
    </Card>
    <Modal v-model="show_Modal" width="80%">
      <p slot="header" style="text-align:center">

        <span>现场信息反馈</span>
      </p>
      <div>
        <Form ref="infoForm" :model="infoForm" :label-width="100" inline>
          <FormItem label="标题" prop="infoTitle" style="width:90%">
            <Input v-model="infoForm.infoTitle"/>
          </FormItem>
          <FormItem label="医院名称" prop="hospName" style="width:45%">
            <Input v-model="infoForm.hospName"/>
          </FormItem>
          <FormItem label="供应商" prop="supplierName" style="width:45%">
            <Input v-model="infoForm.supplierName"></Input>
          </FormItem>
          <FormItem label="应用科室" prop="hospDept" style="width:45%">
            <Input v-model="infoForm.hospDept"/>
          </FormItem>
          <FormItem label="供应商联系人" prop="supplierContact" style="width:45%">
            <Input v-model="infoForm.supplierContact"/>
          </FormItem>
          <FormItem label="场景描述" prop="infoDetail" style="width:90%">
            <Input v-model="infoForm.infoDetail" type="textarea" :autosize="{minRows: 5,maxRows: 10}" placeholder="提出硬件使用的场景（硬件安装的系统、使用场景、使用人员等）"/>
          </FormItem>
        </Form>

      </div>
      <div slot="footer">
        <Button type="primary" @click="handleSubmit('infoForm',1)" :loading="modal_loading">提交</Button>
        <Button type="success" @click="handleSubmit('infoForm',0)" :loading="modal_loading">保存</Button>
        <Button type="warning" @click="handleReset('infoForm')" style="margin-left: 8px">重置</Button>
        <Button @click="show_Modal=false" style="margin-left: 8px">关闭</Button>

      </div>
    </Modal>
  </div>
</template>

<script>
import {
  getInfoList
} from '@/api/data'
export default {
  name: 'sales-information-feedback',
  data () {
    return {
      infoData: [],
      infoColumns: [],
      onSelect: {
        row: null,
        index: -1
      },
      show_Modal: false,
      modal_loading: false,
      infoForm: {
        infoTitle: '',
        hospName: '',
        hospDept: '',
        supplierName: '',
        supplierContact: '',
        infoDetail: '',
        infoType: '',
        infoStatus: '',
        createTime: ''
      }
    }
  },
  mounted () {
    this.setInfoColumns()
    this.getinfoData()
  },

  methods: {

    getinfoData () {
      getInfoList().then(res => {
        this.infoData = res.data.filter(item => {
          return item.infoType === '销售'
        })
      })
    },

    setInfoColumns () {
      this.infoColumns = [{
        title: '标题',
        key: 'infoTitle',
        tooltip: true
      },
      {
        title: '医院名称',
        key: 'hospName'
      },
      {
        title: '科室',
        key: 'hospDept'
      },
      {
        title: '供应商',
        key: 'supplierName'
      },
      {
        title: '联系人',
        key: 'supplierContact'
      },
      {
        title: '状态',
        key: 'infoStatus',
        render: (h, params) => {
          const row = params.row
          const text = row.infoStatus === 0 ? '未提交' : '已提交'
          const color = row.infoStatus === 0 ? 'red' : 'green'
          return h('p', {
            style: {
              color: color
            }
          }, text)
        }
      },
      {
        title: '描述',
        width: 80,
        render: (h, params) => {
          return h('tooltip', [

            h('Icon', {
              props: {
                type: this.infoData[params.index].infoDetail.length > 0 ? 'ios-eye' : '',
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
              this.infoData[params.index].infoDetail.length > 0 ? '点击预览' : '无描述信息'
            )
          ])
        }
      },

      {
        title: '创建时间',
        key: 'createTime'
      }

      ]
    },
    show (index) {
      if (this.infoData[index].infoDetail.length > 0) {
        this.$Modal.info({
          scrollable: true,
          closable: true,
          width: 800,
          render: h => {
            return h(
              'div', {
                style: {
                  margin: '15px'
                }
              },
              this.infoData[index].infoDetail
            )
          }
        })
      }
    },
    onRowClick (row, index) {
      this.onSelect.row = row
      this.onSelect.index = index
    },
    onRowDblclick (row, index) {
      this.onSelect.row = row
      this.onSelect.index = index
      this.editInfo()
    },
    addInfo () {
      this.$refs.tables.clearCurrentRow() // 清除 表格选中高亮
      this.$refs.infoForm.resetFields() // 重置表单

      this.show_Modal = true
      this.onSelect.index = -1 // 清除表格索引
    },
    editInfo () {
      if (this.onSelect.index < 0) {
        this.$Message.error('请选择行！')
        return
      }
      if (this.onSelect.row.infoStatus === 1) {
        this.$Message.error('该需求已提交,无法进行编辑！')
        return
      }

      this.infoForm = this.onSelect.row
      this.show_Modal = true
    }

  }

}

</script>
<style lang='less' scoped>
  .sales-information-feedback {
    .tools {
      margin: 0 0 5 0;

      Button {
        margin: 5px;
      }
    }

  }

</style>
