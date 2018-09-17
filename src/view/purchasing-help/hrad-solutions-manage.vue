<template>

  <div class="hrad-solutions-manage">

    <Row :gutter="20">
      <Col span="16">
      <Card>

        <Transfer :data="hardList" :target-keys="targetKeys4" :render-format="render4" @on-change="handleChange4"
          :list-style="listStyle" filterable></Transfer>
      </Card>
      </Col>
    </Row>
  </div>
</template>
<script>
  import {
    getProLine,
    getHardType,
    getHardList
  } from '@/api/data'
  export default {
    name: 'hrad-solutions-manage',
    data() {
      return {
        hardList: this.getMockData(),
        targetKeys4: this.getTargetKeys(),
        listStyle: {
          width: '250px',
          height: '300px'
        }
      }
    },
    mounted() {
      // 硬件列表
      getHardList().then(res => {
        var hardList = res.data

        this.hardList = hardList.map(item => {


        })

      })
    },
    methods: {
      getMockData() {
        let mockData = [];
        for (let i = 1; i <= 20; i++) {
          mockData.push({
            key: i.toString(),
            label: 'Content ' + i,
            description: 'The desc of content  ' + i,
          });
        }
        return mockData;
      },
      getTargetKeys() {
        return this.getMockData()
          .filter(() => Math.random() * 2 > 1)
          .map(item => item.key);
      },
      handleChange4(newTargetKeys) {
        this.targetKeys4 = newTargetKeys;
      },
      render4(item) {
        return item.label + ' - ' + item.description;
      }


    }
  }

</script>
