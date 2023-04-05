<template>
  <el-container>
    <el-header offset="5">图书借阅管理系统
      <el-dropdown size="small">
        <span class="el-dropdown-link">
          <el-avatar style="cursor: pointer;">{{ UName }}</el-avatar>
        </span>
        <el-dropdown-menu slot="dropdown">
          <el-dropdown-item @click.native="logout">退出登录</el-dropdown-item>
        </el-dropdown-menu>
      </el-dropdown>
    </el-header>
    <el-container>

      <el-aside width="210px">
        <el-menu router background-color="" text-color="#000" active-text-color="#409EFF">
          <el-submenu index="3">
            <template slot="title">
              <i class="el-icon-notebook-1"></i>
              <span>馆藏信息管理</span>
            </template>
            <el-menu-item index="/book">
              <i class="el-icon-notebook-2"></i>
              <span slot="title">书籍信息管理</span>
            </el-menu-item>
            <el-menu-item index="/type">
              <i class="el-icon-collection"></i>
              <span slot="title">书籍类别管理</span>
            </el-menu-item>
          </el-submenu>
          <el-submenu index="2">
            <template slot="title">
              <i class="el-icon-document"></i>
              <span>借阅信息管理</span>
            </template>
            <el-menu-item index="/borrow">
              <i class="el-icon-files"></i>
              <span slot="title">借阅信息管理</span>
            </el-menu-item>
            <el-menu-item index="/reader">
              <i class="el-icon-place"></i>
              <span slot="title">读者信息管理</span>
            </el-menu-item>
          </el-submenu>
          <el-submenu v-if="this.UIdentity == 0" index="1">
            <template slot="title">
              <i class="el-icon-s-tools"></i>
              <span>系统信息管理</span>
            </template>
            <el-menu-item index="/user">
              <i class="el-icon-user"></i>
              <span slot="title">用户信息管理</span>
            </el-menu-item>
            <el-menu-item index="/logs">
              <i class="el-icon-document-copy"></i>
              <span slot="title">系统日志信息</span>
            </el-menu-item>
          </el-submenu>

        </el-menu>
      </el-aside>

      <el-container>
        <el-main>
          <router-view/>
        </el-main>
        <el-footer style="height: 37px">@FuJiTsu</el-footer>
      </el-container>
    </el-container>
  </el-container>
</template>

<script>

import request from "../utils/request";

export default {
  data() {
    return {
      UIdentity: 0,
      UName: ''
    }
  },
  methods: {
    logout() {
      request.get('Login/UserLogout');
      this.$message.info('登出');
      this.$router.push('/');
      sessionStorage.clear();
    },
    isUIdentity() {
      this.UIdentity = sessionStorage.getItem('UIdentity');
      this.UName = sessionStorage.getItem('UName');
    }
  },
  mounted() {
    this.isUIdentity();
  }
}
</script>

<style scoped>
.el-header {
  background-color: #66b1ff;
  display: flex;
  justify-content: space-between;
  padding-left: 10px;
  align-items: center;
  color: white;
  font-size: 18px;
}

.el-aside {
  color: white;
  background-color: #fff;
  height: 100%;
}

.el-menu {
  color: white;
  background-color: #fff;
}

.el-main {
  background-color: aliceblue;
}

.el-container {
  height: 100%;
}

.el-menu {
  /* 解决1px elementui出现展开后子菜单宽度多出1px问题 border: none; */
  border-right-width: 0;
}

.el-footer {
  background-color: aliceblue;
}
</style>
