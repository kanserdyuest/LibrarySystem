<template>
  <el-card>
    <el-breadcrumb separator="/">
      <el-breadcrumb-item :to="{ path: '/main' }">首页</el-breadcrumb-item>
      <el-breadcrumb-item>系统信息管理</el-breadcrumb-item>
      <el-breadcrumb-item>用户信息管理</el-breadcrumb-item>
    </el-breadcrumb>

    <el-row :gutter="20">
      <el-col :span="8">
        <el-input placeholder="请输入用户名查询" v-model="pager.UName" @change="listPage" clearable></el-input>
      </el-col>
      <el-col :span="3">
        <el-button type="primary" icon="el-icon-plus" @click="openAddDialog">添加</el-button>
      </el-col>
      <el-tooltip content="刷新" placement="left">
        <el-button icon="el-icon-refresh" circle style="position:absolute;right:20px;" @click="reload"></el-button>
      </el-tooltip>
    </el-row>
    <el-table :data="tableData" style="width: 100%" v-loading="loading">
      <el-table-column prop="UName" label="用户名" align="center">
      </el-table-column>
      <el-table-column label="密码" align="center">******</el-table-column>
      <el-table-column prop="UGender" label="性别" align="center">
        <template slot-scope="scope">
          <el-tag v-if="scope.row.UGender === 0" type="danger" effect="plain">女</el-tag>
          <el-tag v-if="scope.row.UGender === 1" type="" effect="plain">男</el-tag>
        </template>
      </el-table-column>
      <el-table-column prop="UBirthday" label="出生日期" align="center">
      </el-table-column>
      <el-table-column prop="UPhone" label="电话" align="center">
      </el-table-column>
      <el-table-column width="260" prop="UIdentity" label="身份" align="center">
        <template v-slot="scope">
          <el-switch v-model="scope.row.UIdentity" :active-value=0 :inactive-value=1 active-text="系统管理员"
            inactive-text="图书管理员" @change="changeUIdentity(scope.row)">
          </el-switch>
        </template>
      </el-table-column>
      <el-table-column label="操作" width="180px" align="center">
        <template slot-scope="scope">
          <el-tooltip content="点击修改" placement="top">
            <el-button type="warning" icon="el-icon-edit" circle @click="openEditDialog(scope.row)"></el-button>
          </el-tooltip>
          <el-tooltip content="点击删除" placement="top">
            <el-button v-if="scope.row.UIdentity" type="danger" icon="el-icon-delete" circle @click="openRemoveDialog(scope.row)"></el-button>
          </el-tooltip>
          <el-tooltip content="点击修改密码" placement="top">
            <el-button type="primary" icon="el-icon-lock" circle @click="openPwdDialog(scope.row)"></el-button>
          </el-tooltip>
        </template>
      </el-table-column>
    </el-table>
    <el-pagination @size-change="handleSizeChange" @current-change="handleCurrentChange" :current-page="pager.page"
      :page-sizes="[5, 10, 15, 20]" :page-size="pager.size" layout="total, sizes, prev, pager, next, jumper"
      :total="pager.total">
    </el-pagination>


    <el-dialog title="添加" :visible.sync="addFlag" :show-close="false" width="800px" :close-on-click-modal="false">
      <el-form :model="user" ref="user" label-width="100px" :rules="rules">
        <el-row>
          <el-col :span="10">
            <el-form-item label="用户名：" prop="UName">
              <el-input v-model="user.UName"></el-input>
            </el-form-item>
          </el-col>
          <el-col :span="2">&nbsp;</el-col>
          <el-col :span="10">
            <el-form-item label="密码：" prop="UPwd">
              <el-input type="password" v-model="user.UPwd"></el-input>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="10">
            <el-form-item label="电话号码：" prop="UPhone">
              <el-input v-model="user.UPhone"></el-input>
            </el-form-item>
          </el-col>
          <el-col :span="2">&nbsp;</el-col>
          <el-col :span="10">
            <el-form-item label="选择性别:" prop="UGender">
              <template>
                <el-select v-model="user.UGender" placeholder="请选择">
                  <el-option label="男" value="1">男</el-option>
                  <el-option label="女" value="0">女</el-option>
                </el-select>
              </template>
            </el-form-item>
          </el-col>

        </el-row>
        <el-row>
          <el-col :span="10">
            <el-form-item label="用户身份：" prop="UIdentity">
              <template>
                <el-select v-model="user.UIdentity" placeholder="请选择">
                  <el-option label="系统管理员" value="0">系统管理员</el-option>
                  <el-option label="图书管理员" value="1">图书管理员</el-option>
                </el-select>
              </template>
            </el-form-item>
          </el-col>
          <el-col :span="2">&nbsp;</el-col>
          <el-col :span="10">
            <el-form-item label="出生日期：" prop="UBirthday">
              <el-date-picker v-model="user.UBirthday" type="date" placeholder="选择日期时间">
              </el-date-picker>
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="addFlag = false">关闭</el-button>
        <el-button type="primary" @click="save">保存</el-button>
      </div>
    </el-dialog>

    <el-dialog title="修改密码" :visible.sync="pwdFlag" :show-close="false" width="500px" :close-on-click-modal="false">
      <el-form :model="pwd" ref="pwd" label-width="100px" :rules="pwdrules">
        <el-row>
          <el-col :span="18">
            <el-form-item label="新密码" prop="newPwd">
              <el-input type="password" v-model="pwd.newPwd"></el-input>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="18">
            <el-form-item label="确认密码" prop="confirmPwd">
              <el-input type="password" v-model="pwd.confirmPwd"></el-input>
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="pwdFlag = false">关闭</el-button>
        <el-button type="primary" @click="pwdEdit">保存</el-button>
      </div>
    </el-dialog>

    <el-dialog title="修改" :visible.sync="editFlag" :show-close="false" width="800px" :close-on-click-modal="false">
      <el-form :model="user" ref="user" label-width="100px" :rules="rules">
        <el-row>
          <el-col :span="10">
            <el-form-item label="用户名：" prop="UName">
              <el-input v-model="user.UName"></el-input>
            </el-form-item>
          </el-col>
          <el-col :span="2">&nbsp;</el-col>
          <el-col :span="10">
            <el-form-item label="电话号码：" prop="UPhone">
              <el-input v-model="user.UPhone"></el-input>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="10">
            <el-form-item label="选择性别:" prop="UGender">
              <template>
                <el-select v-model="user.UGender" placeholder="请选择">
                  <el-option label="男" value="1"></el-option>
                  <el-option label="女" value="0"></el-option>
                </el-select>
              </template>
            </el-form-item>
          </el-col>
          <el-col :span="2">&nbsp;</el-col>
          <el-col :span="10">
            <el-form-item label="出生日期：" prop="UBirthday">
              <el-date-picker v-model="user.UBirthday" type="date" placeholder="选择日期时间">
              </el-date-picker>
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="editFlag = false">关闭</el-button>
        <el-button type="primary" @click="edit">保存</el-button>
      </div>
    </el-dialog>


  </el-card>
</template>

<script>
import { getUserList, saveUser, editUser, removeUser, updateUIdentity, updatePwd } from '../api/user'

export default {
  data() {
    var validatePass = (rule, value, callback) => {
      if (value === '') {
        callback(new Error('请输入密码'));
      } else {
        if (this.pwd.confirmPwd !== '') {
          this.$refs.pwd.validateField('confirmPwd');
        }
        callback();
      }
    };
    var validatePass2 = (rule, value, callback) => {
      if (value === '') {
        callback(new Error('请再次输入密码'));
      } else if (value !== this.pwd.newPwd) {
        callback(new Error('两次输入密码不一致!'));
      } else {
        callback();
      }
    }
    return {
      loading:false,
      rules: {
        UName: [{
          required: true,
          message: '请输入用户名',
          trigger: 'change'
        }],
        UPwd: [{
          required: true,
          message: '请输入密码',
          trigger: 'change'
        }],
        UPhone: [{
          required: true,
          message: "请输入手机号码",
          trigger: "blur"
        },
        {
          validator: function (rule, value, callback) {
            if (/^1[34578]\d{9}$/.test(value) == false) {
              callback(new Error("手机号格式错误"));
            } else {
              callback();
            }
          },
          trigger: "blur"
        }],
        UGender: [{
          required: true,
          message: '请选择性别',
          trigger: 'change'
        }],
        UIdentity: [{
          required: true,
          message: '请选择用户身份',
          trigger: 'change'
        }],
        UBirthday: [{
          required: true,
          message: '请输入出生日期',
          trigger: 'change'
        }]
      },
      pwdrules: {
        newPwd: [
          { required: true, message: '请输入密码', trigger: 'blur' },
          { min: 6, max: 16, message: '长度在 6 到 16 个字符', trigger: 'blur' },
          { validator: validatePass, trigger: 'blur' }
        ],
        confirmPwd: [
          { required: true, message: '请确认密码', trigger: 'blur' },
          { min: 6, max: 16, message: '长度在 6 到 16 个字符', trigger: 'blur' },
          { validator: validatePass2, trigger: 'blur', required: true }
        ],
      },
      readValue: '',
      addFlag: false,
      editFlag: false,
      pwdFlag: false,
      tableData: [],
      pager: {
        page: 1,
        size: 5,
        total: 0,
        UName: '',
      },
      user: {
        UId: 0,
        UName: '',
        UPwd: '',
        UGender: '',
        UBirthday: '',
        UPhone: '',
        UIdentity: ''
      },
      pwd: {
        newPwd: '',
        confirmPwd: ''
      }
    }
  },
  methods: {
    reload() {
      this.loading = true;
      setTimeout(() => this.loading = false, 500);
      this.listPage();
    },
    openAddDialog() {
      this.addFlag = !this.addFlag;
      this.user = {
        UId: 0,
        UName: '',
        UPwd: '',
        UGender: '',
        UBirthday: '',
        UPhone: '',
        UIdentity: ''
      }
    },
    openEditDialog(row) {
      this.editFlag = !this.editFlag;
      this.user = row;
      this.readValue = this.user.UGender;
      this.user.UGender = String(this.readValue)
    },
    openPwdDialog(row) {
      this.pwdFlag = !this.pwdFlag;
      this.user.UId = row.UId;
      this.pwd = {
        newPwd: '',
        confirmPwd: ''
      }
    },
    handleCurrentChange(val) {
      console.log(`当前页: ${val}`);
      this.pager.page = val;
      this.listPage();
    },
    handleSizeChange(val) {
      console.log(`每页 ${val} 条`);
      this.pager.size = val;
      this.listPage()
    },
    listPage() {
      getUserList({
        page: this.pager.page,
        size: this.pager.size,
        UName: this.pager.UName
      }).then(res => {
        if (res.data.code === 200) {
          this.tableData = res.data.data;
          this.pager.total = res.data.count;
        }
      }).catch(error => {
        this.$message.error('网络异常');
      })
    },
    save() {
      this.$refs['user'].validate(valid => {
        if (valid) {
          saveUser(this.user).then(res => {
            if (res.data.code === 200) {
              this.$message({
                message: res.data.msg,
                type: "success"
              })
              this.listPage();
              this.addFlag = !this.addFlag; //关闭添加对话框
              this.user = {
                UId: 0,
                UName: '',
                UPwd: '',
                UGender: '',
                UBirthday: '',
                UPhone: '',
                UIdentity: ''
              }
            } else if (res.data.code === 555) {
              this.$message({
                message: res.data.msg,
                type: "warning"
              })
            }
          }).catch(error => {
            this.$message.error('错了哦，网络异常');
          })
        }
      })
    },

    edit() {
      this.$refs['user'].validate(valid => {
        if (valid) {
          editUser(this.user).then(res => {
            if (res.data.code === 200) {
              this.$message({
                message: res.data.msg,
                type: "success"
              })
              this.listPage();
              this.editFlag = !this.editFlag; //关闭添加对话框
              this.user = {
                UId: 0,
                UName: '',
                UPwd: '',
                UGender: '',
                UBirthday: '',
                UPhone: '',
                UIdentity: ''
              }
            }
          }).catch(error => {
            this.$message.error('错了哦，网络异常');
          })
        }
      })
    },
    openRemoveDialog(row) {
      this.$confirm('此操作将永久删除该用户, 是否继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        this.remove(row);
      }).catch(() => {
        this.$message({
          type: 'info',
          message: '已取消删除'
        });
      });
    },
    remove(row) {
      removeUser(row.UId).then(res => {
        if (res.data.code === 200) {
          this.listPage();
          this.$message({
            message: res.data.msg,
            type: 'success'
          });
        }
      }).catch(error => {
        this.$message.error('错了哦，网络异常');
      })
    },
    changeUIdentity(row) {
      updateUIdentity(row.UId, row.UIdentity).then(res => {
        if (res.data.code === 200) {
          this.listPage();
          this.$message({
            message: res.data.msg,
            type: 'success'
          });
        }
      })
    },
    pwdEdit() {
      this.$refs['pwd'].validate(valid => {
        if (valid) {
          updatePwd(this.user.UId, this.pwd.newPwd).then(res => {
            if (res.data.code === 200) {
              this.$message({
                message: res.data.msg,
                type: "success"
              })
              this.listPage();
              this.pwdFlag = !this.pwdFlag; //关闭添加对话框
              this.pwd = {
                newPwd: '',
                confirmPwd: ''
              },
                this.user.UId = 0;
            } else if (res.data.code === 555) {
              this.$message({
                message: res.data.msg,
                type: "warning"
              })
            }
          }).catch(error => {
            this.$message.error('错了哦，网络异常');
          })
        }
      })
    },

  },

  mounted() {
    this.listPage();
  }

}
</script>

<style scoped>
.el-breadcrumb {
  margin-bottom: 20px;
}

.el-pagination {
  margin-top: 10px;
}
</style>
