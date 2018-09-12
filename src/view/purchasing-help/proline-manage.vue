<template>
  <div class="sales-information-feedback">
    <Card>
      <div class="tools">
        <Button type="primary" @click="addInfo">新增</Button>
        <Button type="primary" @click="editInfo">编辑</Button>
      </div>
      <Table :data="proLineData" :columns="proLineColumns"  ref="tables" :highlight-row="true" @on-row-click="onRowClick"
        @on-row-dblclick="onRowDblclick"></Table>
      <div style="margin: 10px;overflow: hidden">
        <div style="float: right;">
          <Page :total="1" :current="1"></Page>
        </div>
      </div>
    </Card>
    <Modal v-model="show_Modal">
      <p slot="header" style="text-align:center">

        <span>产品线</span>
      </p>
      <div>
        <Form ref="infoForm" :model="infoForm" :label-width="100">
          <FormItem label="产品线名称" prop="lineName" >
            <Input v-model="infoForm.lineName"/>
          </FormItem>
          <FormItem label="版本号" prop="versions">
            <Input v-model="infoForm.versions"/>
          </FormItem>
          <FormItem label="说明" prop="remark">
            <Input v-model="infoForm.remark" type="textarea" :autosize="{minRows: 2,maxRows: 5}"/>
          </FormItem>
        </Form>

      </div>
      <div slot="footer">
        <Button type="primary" @click="handleSubmit('infoForm',0)" :loading="modal_loading">保存</Button>
        <Button @click="show_Modal=false" style="margin-left: 8px">关闭</Button>
      </div>
    </Modal>
  </div>
</template>

<script>
export default {
  name: 'proline-manage',
  data () {
    return {
      proLineData: [
        { lineName: '麻醉', versions: '5.0', remark: '' },
        { lineName: 'ICU', versions: '5.0', remark: '' },
        { lineName: '麻醉一体机', versions: '', remark: '' },
        { lineName: 'ICU', versions: '6.0', remark: '' },
        { lineName: 'WiiCare', versions: '1.0', remark: '' },
        { lineName: '急诊急救', versions: '1.0', remark: '' },
        { lineName: '数字化', versions: '', remark: '' }
      ],
      proLineColumns: [],
      onSelect: {
        row: null,
        index: -1
      },
      show_Modal: false,
      modal_loading: false,
      infoForm: {
        lineName: '',
        versions: '',
        remark: ''
      }
    }
  },
  mounted () {
    this.setInfoColumns()
  },

  methods: {

    setInfoColumns () {
      this.proLineColumns = [
        {
          title: '产品线名称',
          key: 'lineName',
          tooltip: true
        },
        {
          title: '版本号',
          key: 'versions'
        },
        {
          title: '备注',
          key: 'remark'
        }
      ]
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

    button {
      margin: 5px;
    }
  }
}
</style>
