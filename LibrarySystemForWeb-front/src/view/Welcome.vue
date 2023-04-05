<template>
  <div>
    <!-- <el-carousel :interval="5000" arrow="always">
      <el-carousel-item v-for="item in imgBox" :key="item">
        <img :src="item.idImg" class="image">
      </el-carousel-item>
    </el-carousel> -->
    <el-card>

<!--  
  <el-row :gutter="20">
        <el-col :span="2">&nbsp;</el-col>
        <el-col :span="5">
          <el-card class="box-card" shadow="hover">
            <div slot="header" class="clearfix">
              <span>卡片名称</span>
            </div>
            <div class="text item">
              列表内容
            </div>
          </el-card>
        </el-col>
        <el-col :span="5">
          <el-card class="box-card" shadow="hover">
            <div slot="header" class="clearfix">
              <span>卡片名称</span>
            </div>
            <div class="text item">
              列表内容
            </div>
          </el-card>
        </el-col>
        <el-col :span="5">
          <el-card class="box-card" shadow="hover">
            <div slot="header" class="clearfix">
              <span>卡片名称</span>
            </div>
            <div class="text item">
              列表内容
            </div>
          </el-card>
        </el-col>
        <el-col :span="5">
          <el-card class="box-card" shadow="hover">
            <div slot="header" class="clearfix">
              <span>卡片名称</span>
            </div>
            <div class="text item">
              列表内容
            </div>
          </el-card>
        </el-col>
        <el-col :span="2">&nbsp;</el-col>
      </el-row> 
-->

      <el-row :gutter="10">
        <el-col :span="11">
          <div style="height: 30px;"></div>
          <div id="chart1" style="height: 600px;width: 800px;"></div>
        </el-col>
        <el-col :span="2">&nbsp;</el-col>
        <el-col :span="11">
          <div style="height: 30px;"></div>
          <div id="chart3" style="height: 400px;width: 700px;"></div>
          <div id="chart2" style="height: 300px;width: 700px;"></div>
        </el-col>
      </el-row>
    </el-card>
  </div>
</template>

<script>
import { getBookTypeChatrs, getBookNumChatrs, getBorrowTop } from '../api/chartrs'

export default {

  data(){
    return{

    }
  },

  methods: {
    loadBookChart() {
      var chartDom = document.getElementById('chart1');
      var myChart = this.$echarts.init(chartDom);
      var option;
      var legendData = [];
      var seriesData = [];
      getBookNumChatrs().then(res => {
        if (res.data.code == 200) {
          let temp = res.data.data;
          for (let i = 0; i < temp.length; i++) {
            seriesData.push({ name: temp[i].BiName, value: temp[i].BiNum });
            legendData.push(temp[i].BiName);
          }
          option = {
            title: {
              text: '在库书籍数量统计',
              subtext: '实时数据',
              left: 'center',
            },
            tooltip: {
              trigger: 'item',
              formatter: '{a} <br/>{b} : {c}本 ({d}%)'
            },
            legend: {
              type: 'scroll',
              orient: 'vertical',
              right: 10,
              top: 20,
              bottom: 20,
              data: legendData
            },
            series: [
              {
                name: '书名',
                type: 'pie',
                radius: '55%',
                center: ['40%', '50%'],
                data: seriesData,
                emphasis: {
                  itemStyle: {
                    shadowBlur: 10,
                    shadowOffsetX: 0,
                    shadowColor: 'rgba(0, 0, 0, 0.5)'
                  }
                }
              }
            ]
          };
          option && myChart.setOption(option);
        }
      }).catch(error => {
        this.$message.error('网络异常');
      })
    },
    loadBookTypeChart() {
      let chartDom = document.getElementById('chart2');
      let myChart = this.$echarts.init(chartDom);
      let option;
      let dataAxis = [];
      let data = [];
      getBookTypeChatrs().then(res => {
        if (res.data.code == 200) {
          let temp = res.data.data;
          for (let i = 0; i < temp.length; i++) {
            dataAxis.push(temp[i].BtName);
            data.push(temp[i].Sum);
          }
          option = {
            title: {
              text: '在库书籍类别统计',
              subtext: '',
              left: 'center'
            },
            xAxis: {
              data: dataAxis,
              axisLabel: {
                inside: true,
                color: '#fff'
              },
              axisTick: {
                show: false
              },
              axisLine: {
                show: false
              },
              z: 10
            },
            yAxis: {
              axisLine: {
                show: false
              },
              axisTick: {
                show: false
              },
              axisLabel: {
                color: '#999'
              }
            },
            tooltip: { // 鼠标悬浮提示框显示 X和Y 轴数据
              trigger: 'axis',
              backgroundColor: 'rgba(32, 33, 36,.7)',
              borderColor: 'rgba(32, 33, 36,0.20)',
              borderWidth: 1,
              textStyle: { // 文字提示样式
                color: '#fff',
                fontSize: '12'
              },
            },
            dataZoom: [
              {
                type: 'inside'
              }
            ],
            series: [
              {
                type: 'bar',
                showBackground: true,
                itemStyle: {
                  color: new this.$echarts.graphic.LinearGradient(0, 0, 0, 1, [
                    { offset: 0, color: '#83bff6' },
                    { offset: 0.5, color: '#188df0' },
                    { offset: 1, color: '#188df0' }
                  ])
                },
                emphasis: {
                  itemStyle: {
                    color: new this.$echarts.graphic.LinearGradient(0, 0, 0, 1, [
                      { offset: 0, color: '#2378f7' },
                      { offset: 0.7, color: '#2378f7' },
                      { offset: 1, color: '#83bff6' }
                    ])
                  }
                },
                data: data
              }
            ]
          };
          option && myChart.setOption(option);
        }
      })
    },
    loadBorrowTop() {
      let chartDom = document.getElementById('chart3');
      let myChart = this.$echarts.init(chartDom);
      let option;
      let y_data = [];
      getBorrowTop().then(res => {
        if (res.data.code === 200) {
          let temp = res.data.data;
          for (let i = 0; i < temp.length; i++) {
            y_data.push([temp[i].BiName, temp[i].BoCount]);
          }
          console.log(y_data);
          option = {
            title: {
              text: '近30天借阅排行',
              subtext: '',
              left: 'center'
            },
            dataset: [
              {
                dimensions: ['name', 'score'],
                source: y_data
              },
              {
                transform: {
                  type: 'sort',
                  config: { dimension: 'score', order: 'asc' }
                }
              }
            ],
            xAxis: {},
            yAxis: {
              type: 'category',
              axisLabel: { interval: 0, rotate: 30 }
            },
            series: {
              type: 'bar',
              encode: { y: 'name', x: 'score' },
              datasetIndex: 1
            },
            tooltip: { // 鼠标悬浮提示框显示 X和Y 轴数据
              trigger: 'axis',
              backgroundColor: 'rgba(32, 33, 36,.7)',
              borderColor: 'rgba(32, 33, 36,0.20)',
              borderWidth: 1,
              textStyle: { // 文字提示样式
                color: '#fff',
                fontSize: '12'
              },
            }
          };
          option && myChart.setOption(option);
        }

      })
    }

  },
  mounted() {
    this.loadBookChart();
    this.loadBookTypeChart();
    this.loadBorrowTop();
  },

}
</script>

<style scoped>
.el-carousel__item h3 {
  color: #475669;
  font-size: 18px;
  opacity: 0.75;
  line-height: 300px;
  margin: 0;
}

.el-carousel__item:nth-child(2n) {
  background-color: #99a9bf;
}

.el-carousel__item:nth-child(2n+1) {
  background-color: #d3dce6;
}
</style>
