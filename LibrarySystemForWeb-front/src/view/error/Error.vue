<template>
    <el-card class="box-card">
        <div slot="header" class="clearfix">
            <span>出错了</span>
        </div>
        <div style="text-align: center;">
            <h2>抱歉，您输入的地址没有找到</h2>
            <h3>o(╥﹏╥)o</h3>
            <el-button @click="goLogin">点击返回主页</el-button>
        </div>
    </el-card>
</template>
  
<script>

export default {

    data() {
        return {
            name: 'notfound',

        }
    },
    methods: {
        goLogin() {
            this.$router.push({
                path: '/main'
            })
        }
    },
}
</script>
  
<style scoped>
.el-card {
    width: 50%;
    position: absolute;
    top: 25%;
    left: 25%;
}
</style>
  