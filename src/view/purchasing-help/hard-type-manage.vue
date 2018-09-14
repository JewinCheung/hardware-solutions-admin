<template>
  <div>
    <Row :gutter="10">
      <Col span="16">
      <Card>
        <Row :gutter="10">
          <Col span="12">
          <Card dis-hover>
            <p slot="title">
              <Icon type="ios-paper-outline"></Icon>
              硬件大类
            </p>
            <ButtonGroup shape="circle" slot="extra">
              <Button icon="md-add"></Button>
              <Button icon="md-create"></Button>
              <Button icon="md-remove"></Button>
            </ButtonGroup>
            <div style="height: 300px;">
              <ul class="iview-admin-draggable-list">
                <li v-for="(item, index) in hardType" :key="index" class="notwrap" :data-index="index" @click="hardTypeClick(item)">
                  {{ item.bigType }}
                </li>
              </ul>
            </div>
          </Card>
          </Col>
          <Col span="12" class="padding-left-10">
          <Card dis-hover>
            <p slot="title">
              <Icon type="ios-list"></Icon>
              硬件小类-[{{hardTypeName}}]
            </p>
            <ButtonGroup shape="circle" slot="extra">
              <Button icon="md-add"></Button>
              <Button icon="md-create"></Button>
              <Button icon="md-remove"></Button>
            </ButtonGroup>
            <div style="height: 300px;">
              <ul id="todoList" class="iview-admin-draggable-list">
                <li v-for="(item, index) in smallType" :key="index" class="notwrap todolist-item" :data-index="index"
                  @click="smallTypeClick(item)">
                  {{item}}
                </li>
              </ul>
            </div>
          </Card>
          </Col>
        </Row>
      </Card>
      </Col>
      <Col span="8" class="padding-left-10">
      <Card>
        <p slot="title">
          <Icon type="ios-paper-outline"></Icon>
          硬件分类
        </p>
        <div style="height: 334px;">

        </div>
      </Card>
      </Col>
    </Row>
    <Row :gutter="10" style="margin-top: 10px;">
      <Col span="16">

      <Card>
        <Row :gutter="10">
          <Col span="12">
          <Card dis-hover id="Anchor_props">
            <p slot="title">
              <Icon type="ios-paper-outline"></Icon>
              参数配置-[{{paramName}}]
            </p>
            <ButtonGroup shape="circle" slot="extra">
              <Button icon="md-add"></Button>
              <Button icon="md-create"></Button>
              <Button icon="md-remove"></Button>
            </ButtonGroup>
            <div style="height: 300px;">
              <ul class="iview-admin-draggable-list">
                <li v-for="(item, index) in paramsData" :key="index" class="notwrap" :data-index="index" @click="paramsClick(item)">
                  {{ item.paramName }}
                </li>
              </ul>
            </div>
          </Card>
          </Col>
          <Col span="12" class="padding-left-10">
          <Card dis-hover>
            <p slot="title">
              <Icon type="ios-list"></Icon>
              配置选项-[{{itemName}}]
            </p>
            <ButtonGroup shape="circle" slot="extra">
              <Button icon="md-add"></Button>
              <Button icon="md-create"></Button>
              <Button icon="md-remove"></Button>
            </ButtonGroup>
            <div style="height: 300px;">
              <ul id="todoList" class="iview-admin-draggable-list">
                <li v-for="(item, index) in itemType" :key="index" class="notwrap todolist-item" :data-index="index">
                  {{ item }}
                </li>
              </ul>
            </div>
          </Card>
          </Col>
        </Row>
      </Card>
      </Col>
      <Col span="8" class="padding-left-10">
      <Card>
        <p slot="title">
          <Icon type="ios-paper-outline"></Icon>
          配置清单
        </p>
        <div style="height: 334px;">
          <ul class="iview-admin-draggable-list">

          </ul>
        </div>
      </Card>
      </Col>
    </Row>
  </div>
</template>

<script>
  export default {
    name: "hard-type-manage",
    data() {
      return {
        hardType: [{
            bigType: "服务器",
            smallType: ["服务器"]
          },
          {
            bigType: "电脑",
            smallType: ["一体机", "台式机", "笔记本", "平板电脑", "瘦客户机"]
          },
          {
            bigType: "打印机",
            smallType: [
              "A3打印机",
              "A4打印机",
              "条码打印机",
              "卡片打印机",
              "针式打印机"
            ]
          },
          {
            bigType: "推车",
            smallType: ["推车"]
          },
          {
            bigType: "电视机",
            smallType: ["电视机"]
          },
          {
            bigType: "支架",
            smallType: ["支架"]
          }
        ],
        paramsType: [{
            smallType: "服务器",
            paramName: "CPU颗数",
            itemName: ["1颗", "2颗", "3颗", "4颗"]
          },
          {
            smallType: "服务器",
            paramName: "CPU核数",
            itemName: ["2.1GHz", "2.4GHz", "2.8GHz", "3.1GHz"]
          },
          {
            smallType: "服务器",
            paramName: "CPU型号",
            itemName: ["E5-2620v4", "Intel Core i5-6500", "I5 六代"]
          },
          {
            smallType: "服务器",
            paramName: "内存",
            itemName: ["4G", "8G", "16G", "32G"]
          },
          {
            smallType: "服务器",
            paramName: "硬盘",
            itemName: ["150G", "250G", "500G", "1T"]
          },
          {
            smallType: "服务器",
            paramName: "操作系统",
            itemName: []
          }
        ],
        hardTypeName: "未选择",
        smallType: [],
        smallTypeName: "未选择",
        paramsData: [],
        paramName: "未选择",
        itemType: [],
        itemName: "未选择"
      };
    },
    mounted() {},

    methods: {
      hardTypeClick(item) {
        this.smallType = item.smallType;
        this.hardTypeName = item.bigType;
        if (item.smallType.length === 0) {
          this.$Message.warning(item.bigType+'-无硬件小类');
          return;
        }
      },
      smallTypeClick(item) {
        const smallTypeName = item;
        this.paramName = smallTypeName;

        this.paramsData = this.paramsType.filter(item => {
          return item.smallType === smallTypeName;
        });
        if (this.paramsData.length === 0) {
          this.$Message.warning(item+'-无参数配置');
          return;
        }

      },
      paramsClick(item) {

        this.itemType = item.itemName;
        this.itemName = item.paramName;
        if (item.itemName.length === 0) {
          this.$Message.warning(item.paramName+'-无配置选项');
          return;
        }
      }
    }
  };

</script>
<style lang='less' scoped>
  .iview-admin-draggable-list {
    height: 100%;
  }

  .iview-admin-draggable-list li {
    padding: 9px;
    border: 1px solid #e7ebee;
    border-radius: 3px;
    margin-bottom: 5px;
    cursor: pointer;
    position: relative;
    transition: all 0.2s;
  }

  .iview-admin-draggable-list li:hover {
    color: #87b4ee;
    border-color: #87b4ee;
    transition: all 0.2s;
  }

  .iview-admin-draggable-delete {
    height: 100%;
    position: absolute;
    right: -8px;
    top: 0px;
    display: none;
  }

  .iview-admin-draggable-list li:hover .iview-admin-draggable-delete {
    display: block;
  }

  .placeholder-style {
    display: block !important;
    color: transparent;
    border-style: dashed !important;
  }

  .delte-item-animation {
    opacity: 0;
    transition: all 0.2s;
  }

  .iview-admin-draggable-list {
    overflow: auto;
  }

</style>
