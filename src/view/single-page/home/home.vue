<template>
  <div class="home">

    <Row :gutter="20">
      <i-col :xs="24" :sm="12" :md="6"v-for="(infor, i) in inforCardData" :key="`infor-${i}`" :style="{'height':'100px','margin-top':'10px'}">
        <infor-card shadow :color="infor.color" :icon="infor.icon" :icon-size="36">
          <count-to :end="infor.count" count-class="count-style" />
          <p>{{ infor.title }}</p>
        </infor-card>
      </i-col>
    </Row>
    <Row :gutter="10" style="margin-top: 20px;" class="home-card">
      <i-col :sm="24" :md="8" style="height:280px">
        <Card>
          <p slot="title" class="card-head">
            销售人员自助
          </p>

          <p class="card-Item">
            <a href="#">硬件库查询</a>
          </p>

          <p class="card-Item">
            <a href="#">现场信息反馈</a>
          </p>

        </Card>
      </i-col>
      <i-col :sm="24" :md="8" style="height:280px">
        <Card>
          <p slot="title" class="card-head">
            产品人员自助
          </p>

          <p class="card-Item">
            <a href="#">硬件库查询</a>
          </p>

          <p class="card-Item">
            <a href="#">硬件需求</a>
          </p>
          <p class="card-Item">
            <a href="#">现场信息反馈</a>
          </p>
        </Card>
      </i-col>
      <i-col :sm="24" :md="8" style="height:280px">
        <Card>
          <p slot="title" class="card-head">
            采购人员自助
          </p>

          <p class="card-Item">
            <a href="#">硬件库查询</a>
          </p>

          <p class="card-Item">
            <a href="#">产品线维护</a>
          </p>
          <p class="card-Item">
            <a href="#">硬件分类维护</a>
          </p>
          <p class="card-Item">
            <a href="#">产品线硬件维护</a>
          </p>
          <p class="card-Item">
            <a href="#">硬件库信息维护</a>
          </p>
        </Card>
      </i-col>
    </Row>
    <Row style="margin-top: 20px;">

    </Row>
  </div>
</template>

<script>
  import InforCard from '_c/info-card'
  import CountTo from '_c/count-to'
  // import { ChartPie, ChartBar } from '_c/charts'
  // import Example from './example.vue'
  export default {
    name: 'home',
    components: {
      InforCard,
      CountTo,
    },
    data() {
      return {
        inforCardData: [{
            title: '硬件数量',
            icon: 'md-map',
            count: 803,
            color: '#2d8cf0'
          },
          {
            title: '大类数量',
            icon: 'md-locate',
            count: 23,
            color: '#19be6b'
          },
          {
            title: '产品线',
            icon: 'md-help-circle',
            count: 7,
            color: '#ff9900'
          },
          {
            title: '信息反馈',
            icon: 'md-share',
            count: 86,
            color: '#ed3f14'
          },
          // { title: '新增互动', icon: 'md-chatbubbles', count: 12, color: '#E46CBB' },
          // { title: '新增页面', icon: 'md-map', count: 14, color: '#9A66E4' }
        ],
      }
    },
    mounted() {
      //
    }
  }

</script>

<style lang="less">
  .count-style {
    font-size: 50px;

  }

  .home {
    padding: 10px;
    .home-card {
      .card-head {
        font-size: 24px;
        line-height: 28px;
        height: 28px;
      }
      .card-Item {
        font-size: 16px;
        margin-top: 10px
      }

      .ivu-card {
        width: 100%;
        height: 100%;
        overflow: hidden;

      }

    }
  }

</style>
