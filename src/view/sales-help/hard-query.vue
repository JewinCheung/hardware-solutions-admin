<template>
  <div class="sales-hard-query">
    <Card>
      <div class="search-con search-con-top">
        <Input clearable search size="large" placeholder="输入关键字搜索" style="width:80%" />
        <Button class="search-btn" type="primary">
          <Icon type="search" />&nbsp;&nbsp;搜索</Button>

      </div>
      <div class="tag-nav-line">
        产品线：
        <Tag v-for="item in columns" :key="`tag-nav-${item.title}`" :name="item.title" :color="item.title === tag_name ? 'primary' : 'default'"
          @click.native="tagClick(item.title)">{{ item.title }}</Tag>
      </div>
      <div class="tag-nav-line">
        硬件分类：
        <Select v-model="model1" style="width:160px">
          <Option v-for="item in cityList" :value="item.value" :key="item.value">{{ item.label }}</Option>
        </Select>
        <Tag v-for="item in columnsT" :key="`tag-nav-${item.title}`" :name="item.title" color="default" >{{ item.title }}</Tag>
      </div>
      <div>
         <Tag closable  v-for="item in columns" :key="`tag-nav-${item.title}`" :name="item.title" color="warning"
          >{{ item.title }}</Tag>
      </div>
    </Card>
  </div>
</template>

<script>
  import Tables from '_c/tables'
  import {
    getTableData
  } from '@/api/data'
  export default {
    name: 'sales-hard-query',
    components: {
      Tables
    },
    data() {
      return {
        columns: [{
            title: '麻醉5.0',
            key: '麻醉5.0',
          },
          {
            title: 'ICU5.0',
            key: 'ICU5.0',
          },
          {
            title: '麻醉一体机',
            key: '麻醉一体机'
          },
          {
            title: 'ICU6.0',
            key: 'ICU6.0'
          },
          {
            title: 'WiiCare1.0',
            key: 'WiiCare1.0'
          },
          {
            title: '急诊急救',
            key: '急诊急救'
          },
          {
            title: '数字化',
            key: '数字化'
          }
        ],
        tableData: [],
        tag_name: "default",
        cityList: [{
            value: '服务器',
            label: '服务器'
          },
          {
            value: '电脑',
            label: '电脑'
          },
          {
            value: '电视机',
            label: '电视机'
          },
          {
            value: '打印机',
            label: '打印机'
          },
          {
            value: '推车',
            label: '推车'
          },
          {
            value: '支架',
            label: '支架'
          },
          {
            value: '扫描枪',
            label: '扫描枪'
          }
        ],
        model1: '电脑',
             columnsT: [{
            title: '台式机',
            key: '台式机',
          },
          {
            title: '笔记本',
            key: '笔记本',
          },
          {
            title: '一体机',
            key: '一体机'
          },
          {
            title: '瘦客户机',
            key: '瘦客户机'
          },
          {
            title: '平板电脑',
            key: '平板电脑'
          }
        ],
        tagCount:[]
      }
    },
    methods: {
      tagClick(name) {
        this.tag_name = name;

      }

    },
    mounted() {
      getTableData().then(res => {
        this.tableData = res.data
      })
    }
  }

</script>

<style lang="less">
  .sales-hard-query {
    .search-con{

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
        border: 1
      }
      .ivu-tag-primary,
        {
        border: 1 !important
      }

    }


  }

</style>
