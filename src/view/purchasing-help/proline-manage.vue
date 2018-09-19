<template>
  <div class="sales-information-feedback">
    <Card>
      <div class="tools">
        <Button type="primary" @click="addInfo">新增</Button>
        <Button type="primary" @click="editInfo">编辑</Button>
      </div>
      <Table :data="proLineData" :columns="proLineColumns" ref="tables" :highlight-row="true" @on-row-click="onRowClick"
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
        <Form ref="infoForm" :model="infoForm" :label-width="100" :rules="ruleValidate">
          <FormItem label="产品线名称:" prop="DictName">
            <Input v-model="infoForm.DictName" />
          </FormItem>
          <FormItem label="版本号:" prop="DictParam">
            <Input v-model="infoForm.DictParam" />
          </FormItem>
          <FormItem label="编号:" prop="DictNo">
            <Input v-model="infoForm.DictNo" />
          </FormItem>
          <FormItem label="序号:">
            <Row>
              <Col span="10">
              <FormItem prop="DictSort">
                <InputNumber :min="1" v-model="infoForm.DictSort" />
              </FormItem>
              </Col>

              <Col span="14">
              <FormItem label="启用:" prop="IsEnabledBtn" :label-width="50">
                <i-switch v-model="infoForm.IsEnabledBtn" size="large">
                  <span slot="open">启用</span>
                  <span slot="close">停用</span>
                </i-switch>
              </FormItem>
              </Col>
            </Row>
          </FormItem>

          <FormItem label="说明:" prop="Memo">
            <Input v-model="infoForm.Memo" type="textarea" :autosize="{minRows: 2,maxRows: 5}" />
          </FormItem>
        </Form>

      </div>
      <div slot="footer">
        <Button type="primary" @click="handleSubmit('infoForm')" :loading="modal_loading">保存</Button>
        <Button @click="show_Modal=false" style="margin-left: 8px">关闭</Button>
      </div>
    </Modal>
  </div>
</template>

<script>
  import {
    productLine
  } from '@/api/data'
  import {
    resData,
    ObjEach
  } from '@/libs/tools'
  export default {
    name: 'proline-manage',
    data() {
      return {
        proLineData: [],
        proLineColumns: [],
        onSelect: {
          row: null,
          index: -1
        },
        show_Modal: false,
        modal_loading: false,
        infoForm: {
          // DictName: '',
          // DictParam: '',
          // DictNo:'',
          DictSort: 1,
          IsEnabledBtn: true,
          // Memo: ''
        },
        ruleValidate: {
          DictName: [{
            required: true,
            message: '不能为空'

          }],
          DictNo: [{
            required: true,
            message: '不能为空'
          }],
          DictSort: [{
            required: true,
            message: '不能为空'
          }],
        }
      }
    },
    mounted() {
      this.setInfoColumns()
    },

    methods: {

      setInfoColumns() {
        this.proLineColumns = [{
            title: '产品线名称',
            key: 'DictName',
            tooltip: true
          },
          {
            title: '版本号',
            key: 'DictParam'
          },
          {
            title: '编号',
            key: 'DictNo'
          },
          {
            title: '序号',
            key: 'DictSort'
          },
          {
            title: '编号',
            key: 'remark'
          },
          {
            title: '是否启用',
            key: 'IsEnabled'
          },
          {
            title: '说明',
            key: 'Memo'
          }
        ]
      },

      onRowClick(row, index) {
        this.onSelect.row = row
        this.onSelect.index = index
      },
      onRowDblclick(row, index) {
        this.onSelect.row = row
        this.onSelect.index = index
        this.editInfo()
      },
      addInfo() {
        this.$refs.tables.clearCurrentRow() // 清除 表格选中高亮
        this.$refs.infoForm.resetFields() // 重置表单

        this.show_Modal = true
        this.onSelect.index = -1 // 清除表格索引
      },
      editInfo() {
        if (this.onSelect.index < 0) {
          this.$Message.error('请选择行！')
          return
        }

        this.infoForm = this.onSelect.row
        this.show_Modal = true
      },
      handleSubmit(name) {
        this.$refs[name].validate((valid) => {
          if (valid) {

            var postData = ObjEach(this.infoForm);
            postData["IsEnabled"] = postData.IsEnabledBtn ? 0 : 1;

            productLine.add(postData).then(res => {

              resData(res, (res) => {

                this.$Message.success(res.Msg);
              }, (res) => {

                this.$Message.success(res.Msg);
              })
            })

          }
        })
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
