<template>
    <div class="loginbBody">
        <div class="loginDiv">
            <div class="login-content">
                <h1 class="login-title">用户登录</h1>
                <el-form label-width="100px" :rules="rules" ref="loginForm" :model="loginForm">
                    <el-form-item label="用户名" prop="UName" required :hide-required-asterisk="true">
                        <el-input style="width: 200px" type="text" v-model="loginForm.UName" autocomplete="off" size="small"
                            prefix-icon="el-icon-user"></el-input>
                    </el-form-item>
                    <el-form-item label="密码" prop="UPwd" required :hide-required-asterisk="true">
                        <el-input style="width: 200px;" type="password" v-model="loginForm.UPwd" show-password
                            autocomplete="off" size="small" prefix-icon="el-icon-key "></el-input>
                    </el-form-item>
                    <el-form-item>
                        <el-button type="primary" @click="login" @keyup.enter="keyDown(e)">确 定 </el-button>
                    </el-form-item>
                </el-form>
            </div>
        </div>
    </div>
</template>


<script>
import request from '../utils/request'
export default {
    data() {
        return {
            loginForm: {
                UName: '',
                UPwd: '',
            },
            rules: {
                UName: [
                    { required: true, message: '请输入用户名', trigger: 'blur' },
                    //{ min: 3, max: 6, message: '用户名长度在 3 到 6 个字符', trigger: 'blur' }
                ],
                UPwd: [
                    { required: true, message: '请输入密码', trigger: 'blur' },
                    //{ min: 3, max: 6, message: '密码长度在 3 到 6 个字符', trigger: 'blur' }
                ]
            }
        }
    },
    methods: {
        login() {
            request({
                url: '/Login/UserLogin', method: 'POST', data: {
                    UName: this.loginForm.UName,
                    UPwd: this.loginForm.UPwd
                },
            }).then(res => {
                if (res.data.code === 200) {
                    sessionStorage.setItem('UName', res.data.data.UName);
                    sessionStorage.setItem('UIdentity', res.data.data.UIdentity);
                    sessionStorage.setItem('code', res.data.code);
                    this.$router.push('/main')
                } else {
                    this.$message.error(res.data.msg);
                }
            }).catch(error => {
                this.$message.error('网络异常');
            })
        },
        keyDown(e) {
            if (e.keyCode == 13 || e.keyCode == 100) {
                this.login();
            }
        }
    },
    mounted() {
        // 绑定监听事件
        window.addEventListener("keydown", this.keyDown);
    },
    destroyed() {
        // 销毁事件
        window.removeEventListener("keydown", this.keyDown, false);
    },
}

</script>




<style scoped>
.loginbBody {
    height: 100%;
    width: 100%;
    background-image: url('../assets/img/bg.jpg');
    /* 背景图垂直、水平均居中 */
    background-position: center center;
    /* 背景图不平铺 */
    background-repeat: no-repeat;
    /* 当内容高度大于图片高度时，背景图像的位置相对于viewport固定 */
    background-attachment: fixed;
    /* 让背景图基于容器大小伸缩 */
    background-size: cover;
    /* 设置背景颜色，背景图加载过程中会显示背景色 */
    background-color: #464646;
}

.loginDiv {
    position: absolute;
    top: 50%;
    left: 50%;
    margin-top: -200px;
    margin-left: -250px;
    width: 450px;
    height: 330px;
    background: #fff;
    border-radius: 5%;
    box-shadow: 0 2px 4px rgba(0, 0, 0, .12), 0 0 6px rgba(0, 0, 0, .04)
}

.login-title {
    margin: 20px 0;
    text-align: center;
    font-family: '微软雅黑';
}

.login-content {
    width: 400px;
    height: 250px;
    position: absolute;
    top: 25px;
    left: 25px;
}

.el-button {
    vertical-align: center;
}
</style>
