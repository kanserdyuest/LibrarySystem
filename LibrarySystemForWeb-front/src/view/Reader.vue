<template>
  <el-card>
    <el-breadcrumb separator="/">
      <el-breadcrumb-item :to="{ path: '/main' }"><i class="el-icon-s-home"></i>首页</el-breadcrumb-item>
      <el-breadcrumb-item>借阅信息管理</el-breadcrumb-item>
      <el-breadcrumb-item>读者信息管理</el-breadcrumb-item>
    </el-breadcrumb>

    <el-row :gutter="20">
      <el-col :span="8">
        <el-input placeholder="请输入读者名查询" v-model="pager.RName" @change="listPage" clearable></el-input>
      </el-col>
      <el-col :span="15">
        <el-button type="primary" icon="el-icon-plus" @click="openAddDialog">读者信息录入</el-button>
      </el-col>
      <el-tooltip content="刷新" placement="left">
        <el-button icon="el-icon-refresh" circle style="position: absolute,right:20px;" @click="reload"></el-button>
      </el-tooltip>
    </el-row>
    <el-table :data="tableData" style="width: 100%" v-loading="loading">
      <el-table-column prop="RName" label="姓名" align="center">
      </el-table-column>
      <el-table-column prop="RGender" label="性别" align="center">
        <template slot-scope="scope">
          <el-tag v-if="scope.row.RGender === 0" type="success" effect="plain">女</el-tag>
          <el-tag v-if="scope.row.RGender === 1" type="danger" effect="plain">男</el-tag>
        </template>
      </el-table-column>
      <el-table-column prop="RBirthday" label="年龄" align="center">
        <template v-slot="scope">
          {{ getAge(scope.row.RBirthday) }}
        </template>
      </el-table-column>
      <el-table-column prop="RPhone" label="电话" align="center">
      </el-table-column>
      <el-table-column prop="REmail" label="邮箱" align="center">
      </el-table-column>
      <el-table-column label="操作" width="180px" align="center">
        <template slot-scope="scope">
          <el-tooltip content="点击修改" placement="top">
            <el-button type="warning" icon="el-icon-edit" circle @click="openEditDialog(scope.row)"></el-button>
          </el-tooltip>
          <el-tooltip content="点击删除" placement="top">
            <el-button type="danger" icon="el-icon-delete" circle @click="openRemoveDialog(scope.row)"></el-button>
          </el-tooltip>
        </template>
      </el-table-column>
    </el-table>
    <el-pagination @size-change="handleSizeChange" @current-change="handleCurrentChange" :current-page="pager.page"
      :page-sizes="[5, 10, 15, 20]" :page-size="pager.size" layout="total, sizes, prev, pager, next, jumper"
      :total="pager.total">
    </el-pagination>

    <el-dialog title="添加" :visible.sync="addFlag" :show-close="false" width="800px" :close-on-click-modal="false">
      <el-form :model="reader" ref="reader" label-width="100px" :rules="rules">
        <el-row>
          <el-col :span="10">
            <el-form-item label="姓名：" prop="RName">
              <el-input v-model="reader.RName"></el-input>
            </el-form-item>
          </el-col>
          <el-col :span="10">
            <el-form-item label="选择性别:" prop="RGender">
              <template>
                <el-select v-model="reader.RGender" placeholder="请选择">
                  <el-option label="男" value="1"></el-option>
                  <el-option label="女" value="0"></el-option>
                </el-select>
              </template>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="10">
            <el-form-item label="电话号码：" prop="RPhone">
              <el-input v-model="reader.RPhone"></el-input>
            </el-form-item>
          </el-col>
          <el-col :span="10">
            <el-form-item label="邮箱：" prop="REmail">
              <el-input v-model="reader.REmail"></el-input>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="8">
            <el-form-item label="出生日期：" prop="RBirthday">
              <el-date-picker v-model="reader.RBirthday" type="date" placeholder="选择日期时间">
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


    <el-dialog title="修改" :visible.sync="editFlag" :show-close="false" width="800px" :close-on-click-modal="false">
      <el-form :model="reader" ref="reader" label-width="100px" :rules="rules">
        <el-row>
          <el-col :span="10">
            <el-form-item label="姓名：" prop="RName">
              <el-input v-model="reader.RName"></el-input>
            </el-form-item>
          </el-col>
          <el-col :span="10">
            <el-form-item label="选择性别:" prop="RGender">
              <template>
                <el-select v-model="reader.RGender" placeholder="请选择">
                  <el-option label="男" value="1"></el-option>
                  <el-option label="女" value="0"></el-option>
                </el-select>
              </template>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="10">
            <el-form-item label="电话号码：" prop="RPhone">
              <el-input v-model="reader.RPhone"></el-input>
            </el-form-item>
          </el-col>
          <el-col :span="10">
            <el-form-item label="邮箱：" prop="REmail">
              <el-input v-model="reader.REmail"></el-input>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="8">
            <el-form-item label="出生日期：" prop="RBirthday">
              <el-date-picker v-model="reader.RBirthday" type="date" placeholder="选择日期时间">
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
import { getReaderList, saveReader, editReader, removeReader } from '../api/reader'

export default {
  data() {
    return {
      loading: false,
      rules: {
        RName: [{
          required: true,
          message: '请输入姓名',
          trigger: 'change'
        }],
        RPhone: [{
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
        REmail: [{
          required: true,
          message: "请输入邮箱",
          trigger: "blur"
        }, {
          validator: function (rule, value, callback) {
            if (/^([a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+((.[a-zA-Z0-9_-]{2,3}){1,2})$/.test(value) == false) {
              callback(new Error("邮箱格式错误"));
            } else {
              callback();
            }
          },
          trigger: "blur"
        }],
        RGender: [{
          required: true,
          message: '请选择性别',
          trigger: 'change'
        }],
        RBirthday: [{
          required: true,
          message: '请输入出生日期',
          trigger: 'change'
        }]
      },
      readValue: '',
      addFlag: false,
      editFlag: false,
      tableData: [],
      pager: {
        page: 1,
        size: 5,
        total: 0,
        RName: '',
      },
      reader: {
        RId: 0,
        RName: '',
        RGender: '',
        RBirthday: '',
        RPhone: '',
        REmail: ''
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
      this.reader = {
        RId: 0,
        RName: '',
        RGender: '',
        RBirthday: '',
        RPhone: '',
        REmail: ''
      }
    },
    openEditDialog(row) {
      this.editFlag = !this.editFlag;
      this.reader = row;
      this.readValue = this.reader.RGender;
      this.reader.RGender = String(this.readValue);
      this.readValue = this.reader.RIdentity;
      this.reader.RIdentity = String(this.readValue);
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
      getReaderList({
        page: this.pager.page,
        size: this.pager.size,
        RName: this.pager.RName
      }).then(res => {
        if (res.data.code === 200) {
          this.tableData = res.data.data;
          this.pager.total = res.data.count;
        }
      }).catch(error => {
        this.$message.error('错了哦，网络异常');
      })
    },
    save() {
      this.$refs['reader'].validate(valid => {
        if (valid) {
          saveReader(this.reader).then(res => {
            if (res.data.code === 200) {
              this.$message({
                message: res.data.msg,
                type: "success"
              })
              this.listPage();
              this.addFlag = !this.addFlag; //关闭添加对话框
              this.reader = {
                RId: 0,
                RName: '',
                UPwd: '',
                RGender: '',
                RBirthday: '',
                RPhone: '',
                RIdentity: ''
              }
            }
          }).catch(error => {
            this.$message.error('错了哦，网络异常');
          })
        }
      })
    },

    edit() {
      this.$refs['reader'].validate(valid => {
        if (valid) {
          editReader(this.reader).then(res => {
            if (res.data.code === 200) {
              this.$message({
                message: res.data.msg,
                type: "success"
              })
              this.listPage();
              this.editFlag = !this.editFlag; //关闭添加对话框
              this.reader = {
                RId: 0,
                RName: '',
                RGender: '',
                RBirthday: '',
                RPhone: '',
                REmail: ''
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
      removeReader(row.RId).then(res => {
        if (res.data.code === 200) {
          this.listPage();
          this.$message({
            message: res.data.msg,
            type: 'success'
          });
        } if(res.data.code === 555){
            this.$message.warning(res.data.msg);
        }
      }).catch(error => {
        this.$message.error('错了哦，网络异常');
      })
    },
    getAge(birthdays) {
      let d = new Date();
      let birthday = new Date(birthdays)
      let age = d.getFullYear() - birthday.getFullYear() - (d.getMonth() < birthday.getMonth() || (d.getMonth() == birthday.getMonth() && d.getDate() < birthday.getDate()) ? 1 : 0);
      return age;
    }
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
