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
        <Tag v-for="item in ProLine" :key="item" :name="item" :color="item === tag_name ? 'primary' : 'default'" @click.native="tagClick(item)">{{ item}}</Tag>
      </div>
      <div class="tag-nav-line">
        硬件分类：
        <Select v-model="bigType" style="width:160px" @on-change="getSmall">
          <Option v-for="item in HardType" :value="item.bigType" :key="item.bigType">{{ item.bigType }}</Option>
        </Select>
        <Tag v-for="item in smallType" :key="`tag-nav-${item}`" :name="item" color="default" @click.native="tagAdd(item)">{{ item }}</Tag>
      </div>
      <div>
        <Tag closable v-for="item in tagCount" :key="item" :name="item.title" color="warning" closable @on-close="tagClose(item)">{{ item}}</Tag>
      </div>
    </Card>
  </div>
</template>

<script>
  import Tables from '_c/tables'
  import {
    getProLine,
    getHardType
  } from '@/api/data'
  export default {
    name: 'sales-hard-query',
    components: {
      Tables
    },
    data() {
      return {
        ProLine: [],//产品线
        HardType: [],//硬件分类
        smallType: [],//硬件小类
        tag_name: "default",//默认标签
        bigType: '',//模式大类
        tagCount: [],
      }
    },
    methods: {

      //切换硬件小分类
      getsmallType() {
        const bigTypeName = this.bigType;

        const HardType = this.HardType.filter((item) => {
          return item.bigType == bigTypeName
        })

        setTimeout(() => {
          this.smallType = HardType[0].smallType;
        }, 300)


      },
      //标签点击事件
      tagClick(name) {
        this.tag_name = name;
        this.tagAdd(name);
      },
      //添加标签
      tagAdd(name) {
        const index = this.tagCount.indexOf(name);
        if (index < 0) {

          this.tagCount.push(name);

        }
      },
      //关闭标签
      tagClose(name) {

        const index = this.tagCount.indexOf(name);
        this.tagCount.splice(index, 1);

      },
      //硬件大分类选择事件
      getSmall(Option) {
        this.bigType = Option;
        this.smallType = [];
        this.getsmallType();
      }

    },
    mounted() {
      //初始化产品线
      getProLine().then(res => {
        this.ProLine = res.data;
        const Line = this.ProLine[0];
        this.tag_name = Line;
        this.tagClick(Line);
      });
      //初始化硬件分类
      getHardType().then(res => {
        this.HardType = res.data
        this.bigType= this.HardType[0].bigType;
        this.getsmallType();

      })

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
        border: 1
      }
      .ivu-tag-primary,
        {
        border: 1 !important
      }

    }


  }

</style>
