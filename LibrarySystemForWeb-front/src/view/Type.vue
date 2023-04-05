<template>

  <el-card>

    <el-breadcrumb separator="/">
      <el-breadcrumb-item :to="{ path: '/main' }"><i class="el-icon-s-home"></i>首页</el-breadcrumb-item>
      <el-breadcrumb-item>馆藏信息管理</el-breadcrumb-item>
      <el-breadcrumb-item>图书类别管理</el-breadcrumb-item>
    </el-breadcrumb>

    <el-row :gutter="20">
      <el-col :span="8">
        <el-input placeholder="请输入图书名查询" v-model="pager.BtName" @change="listPage" clearable></el-input>
      </el-col>
      <el-col :span="3">
        <el-button type="primary" icon="el-icon-plus" @click="openAddDialog">类别信息录入</el-button>
      </el-col>
      <el-tooltip content="刷新" placement="left">
        <el-button icon="el-icon-refresh" circle style="position: absolute;right:20px;"
                   @click="reload"></el-button>
      </el-tooltip>
    </el-row>

    <el-table :data="tableData" style="width: 100%" v-loading="loading">
      <el-table-column prop="BtName" label="类别名称" align="">
      </el-table-column>
      <el-table-column label="操作"  align="center">
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
                   :page-sizes="[5, 10, 15, 20]" :page-size="pager.size"
                   layout="total, sizes, prev, pager, next, jumper"
                   :total="pager.total">
    </el-pagination>

    <el-dialog title="添加" :visible.sync="addFlag" :show-close="false" width="800px" :close-on-click-modal="false">
      <el-form :model="type" ref="type" label-width="100px" :rules="rules">
        <el-row>
          <el-col :span="10">
            <el-form-item label="类别名称：" prop="BtName">
              <el-input v-model="type.BtName"></el-input>
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
      <el-form :model="type" ref="type" label-width="100px" :rules="rules">
        <el-row>
          <el-col :span="10">
            <el-form-item label="类别名称：" prop="BtName">
              <el-input v-model="type.BtName"></el-input>
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
import {editType, getTypeList, removeType, saveType} from "../api/type";
import {editBook} from "../api/book";

export default {
  data() {
    return {
      loading: false,
      rules: {
        BtName: [{
          required: true,
          message: '请输入分类名称',
          trigger: 'change'
        }]
      },
      pager: {
        page: 1,
        size: 5,
        total: 0,
        BtName: '',
      },
      addFlag: false,
      editFlag: false,
      type: {
        BtId: 0,
        BtName: ''
      },
      tableData: []
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
      this.type = {
        BtId: 0,
        BtName: ''
      }
    },
    openEditDialog(row) {
      this.editFlag = !this.editFlag;
      this.type = row;
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
      getTypeList({
        page: this.pager.page,
        size: this.pager.size,
        BtName: this.pager.BtName
      }).then(res => {
        if (res.data.code === 200) {
          this.tableData = res.data.data;
          this.pager.total = res.data.count;
        }
      }).catch(error => {
        this.$message.error('网络异常');
      })
    },
    openRemoveDialog(row) {
      this.$confirm('此操作将永久删除分类，且该分类下所属书籍会受影响，如无特殊情况我们不建议您这么做，是否继续？', '警告', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'error'
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
      removeType(row.BtId).then(res => {
        if (res.data.code === 200) {
          this.listPage();
          this.$message({
            message: res.data.msg,
            type: 'success'
          });
        }
      }).catch(error => {
        this.$message.error('网络异常');
      })
    },
    save() {
      this.$refs['type'].validate(valid => {
        if (valid) {
          saveType(this.type).then(res => {
            if (res.data.code === 200) {
              this.$message({
                message: res.data.msg,
                type: "success"
              })
            }
            this.listPage();
            this.addFlag = !this.addFlag; //关闭添加对话框
            this.type = {
              BtId: 0,
              BtName: '',
            }
          }).catch(error => {
            this.$message.error('网络异常');
          })
        }
      })
    },
    edit() {
      editType(this.type).then(res => {
        if (res.data.code === 200) {
          this.$message({
            message: res.data.msg,
            type: "success"
          })
          this.listPage();
          this.editFlag = !this.editFlag; //关闭添加对话框
          this.type = {
            BtId: 0,
            BtName: '',
          }
        }
      }).catch(error => {
        this.$message.error('网络异常');
      })
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
