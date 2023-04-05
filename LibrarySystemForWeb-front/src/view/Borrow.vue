<template>
  <el-card>
    <el-breadcrumb separator="/">
      <el-breadcrumb-item :to="{ path: '/main' }"><i class="el-icon-s-home"></i>首页</el-breadcrumb-item>
      <el-breadcrumb-item>借阅信息管理</el-breadcrumb-item>
    </el-breadcrumb>

    <el-row :gutter="20">
      <el-col :span="8">
        <el-input placeholder="请输入读者名" v-model="pager.RName" @change="listPage" clearable></el-input>
      </el-col>
      <el-col :span="3">
        <el-button type="primary" icon="el-icon-plus" @click="openAddDialog">借阅新增</el-button>
      </el-col>
      <el-tooltip content="刷新" placement="left">
        <el-button icon="el-icon-refresh" circle style="position: absolute; right: 20px" @click="reload"></el-button>
      </el-tooltip>
    </el-row>
    <el-table :data="tableData" style="width: 100%" v-loading="loading">
      <el-table-column prop="BiName" label="书籍名" align="center">
      </el-table-column>
      <el-table-column prop="RName" label="读者名" align="center">
      </el-table-column>
      <el-table-column prop="BbLendtime" label="借出时间" align="center">
      </el-table-column>
      <el-table-column prop="BbReturntime" label="归还时间" align="center">
      </el-table-column>
      <el-table-column prop="BbRealReturntime" label="实际归还时间" align="center">
      </el-table-column>
      <el-table-column prop="BbIsreborrow" label="是否续借" align="center">
        <template slot-scope="scope">
          <span v-if="scope.row.BbIsreborrow == 1">否</span>
          <span v-else>是</span>
        </template>
      </el-table-column>
      <el-table-column prop="BbReborrowDays" label="续借天数" align="center">
      </el-table-column>
      <el-table-column label="操作" width="180px" align="center">
        <template slot-scope="scope">
          <el-button size="small" type="success" v-if="scope.row.BbIsrenewBook != 1"
            @click="openEditDialog(scope.row)">还书</el-button>
          <el-button size="small" type="info" v-if="scope.row.BbIsrenewBook != 1"
            @click="openRenewDialog(scope.row)">续借</el-button>
          <el-button type="primary" size="small" v-else @click="openDetails(scope.row)">已归还图书</el-button>
        </template>
      </el-table-column>
    </el-table>
    <el-pagination @size-change="handleSizeChange" @current-change="handleCurrentChange" :current-page="pager.page"
      :page-sizes="[5, 10, 15, 20]" :page-size="pager.size" layout="total, sizes, prev, pager, next, jumper"
      :total="pager.total">
    </el-pagination>

    <el-dialog title="新增借阅" :visible.sync="addFlag" :show-close="false" width="800px" :close-on-click-modal="false">
      <el-form :model="borrow" :rules="rules" ref="borrow" label-width="100px">
        <el-row>
          <el-col :span="10">
            <el-form-item label="书籍名：" prop="BiName" clearable>
              <el-input v-model="borrow.BiName" @blur="selectionIsbns($event)"></el-input>
            </el-form-item>
          </el-col>
          <el-col :span="10">
            <el-form-item label="读者名：" prop="RName">
              <el-input v-model="borrow.RName" @blur="selectionPhones($event)"></el-input>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="10">
            <el-form-item label="书籍isbn：" prop="BiIsbn">
              <el-select v-model="borrow.BiIsbn" placeholder="请选择">
                <el-option v-for="item in Isbns" :key="item.Isbn" :label="item.Isbn" :value="item.Isbn">
                </el-option>
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="10">
            <el-form-item label="读者电话：" prop="RPhone">
              <el-select v-model="borrow.RPhone" placeholder="请选择">
                <el-option v-for="item in Phones" :key="item.Phone" :label="item.Phone" :value="item.Phone">
                </el-option>
              </el-select>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="10">
            <el-form-item label="归还时间：" prop="BbReturntime">
              <el-date-picker v-model="borrow.BbReturntime" type="datetime" placeholder="选择日期时间"></el-date-picker>
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="addFlag = false">关闭</el-button>
        <el-button type="primary" @click="save">保存</el-button>
      </div>
    </el-dialog>

    <el-dialog title="还书" :visible.sync="returnFlag" :show-close="false" width="800px" :close-on-click-modal="false">
      <el-form :model="borrow" :rules="rules" ref="borrow" label-width="100px">
        <el-row>
          <el-col :span="12">
            <el-form-item label="书籍名：" prop="BiName">
              <el-input v-model="borrow.BiName" disabled></el-input>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="读者名：" prop="RName">
              <el-input v-model="borrow.RName" disabled></el-input>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="12">
            <el-form-item label="书籍isbn：" prop="BiIsbn">
              <el-input v-model="borrow.BiIsbn" disabled></el-input>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="读者电话：" prop="RPhone">
              <el-input v-model="borrow.RPhone" disabled></el-input>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="12">
            <el-form-item label="借出时间：" prop="BbLendtime">
              <el-date-picker v-model="borrow.BbLendtime" type="datetime" placeholder="选择日期时间" disabled
                style="width: 280px;"></el-date-picker>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="归还时间：" prop="BbReturntime">
              <el-date-picker v-model="borrow.BbReturntime" type="datetime" placeholder="选择日期时间" disabled
                style="width: 280px;"></el-date-picker>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="12">
            <el-form-item label="罚款：" prop="fine">
              <el-input v-model="fine.value" type="number" disabled></el-input>
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="returnFlag = false">关闭</el-button>
        <el-button type="primary" @click="edit1">保存</el-button>
      </div>
    </el-dialog>

    <el-dialog title="续借" :visible.sync="contFlag" :show-close="false" width="800px" :close-on-click-modal="false">
      <el-form :model="borrow" :rules="rules" ref="borrow" label-width="100px">
        <el-row>
          <el-col :span="10">
            <el-form-item label="书籍名：" prop="BiName">
              <el-input v-model="borrow.BiName" disabled></el-input>
            </el-form-item>
          </el-col>
          <el-col :span="10">
            <el-form-item label="读者名：" prop="RName">
              <el-input v-model="borrow.RName" disabled></el-input>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="10">
            <el-form-item label="书籍isbn：" prop="BiIsbn">
              <el-input v-model="borrow.BiIsbn" disabled></el-input>
            </el-form-item>
          </el-col>
          <el-col :span="10">
            <el-form-item label="读者电话：" prop="RPhone">
              <el-input v-model="borrow.RPhone" disabled></el-input>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="10">
            <el-form-item label="借出时间：" prop="BbLendtime">
              <el-date-picker v-model="borrow.BbLendtime" type="datetime" placeholder="选择日期时间" disabled></el-date-picker>
            </el-form-item>
          </el-col>
          <el-col :span="10">
            <el-form-item label="归还时间：" prop="BbReturntime">
              <el-date-picker v-model="borrow.BbReturntime" type="datetime" placeholder="选择日期时间"
                disabled></el-date-picker>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="10">
            <el-form-item label="续借天数：" prop="BbReborrowDays">
              <el-input type="number" v-model.number="borrow.BbReborrowDays" onkeyup="value=value.replace(/^(0+)|[^\d]+/g,'')" ></el-input>
              <!-- <el-input type="number" v-model.number="borrow.BbReborrowDays"></el-input> -->
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="contFlag = false">关闭</el-button>
        <el-button type="primary" @click="edit2">保存</el-button>
      </div>
    </el-dialog>

    <el-dialog title="详情" :visible.sync="detailFlag" :show-close="false" width="800px" :close-on-click-modal="false">
      <el-form :model="borrow" :rules="rules" ref="borrow" label-width="100px">
        <el-row :gutter="5">
          <el-col :span="12">
            <el-form-item label="书籍名：" prop="BiName">
              <el-input class="input" v-model="borrow.BiName" disabled></el-input>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="读者名：" prop="RName">
              <el-input v-model="borrow.RName" disabled></el-input>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row :gutter="5">
          <el-col :span="12">
            <el-form-item label="书籍isbn：" prop="BiIsbn">
              <el-input v-model="borrow.BiIsbn" disabled></el-input>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="读者电话：" prop="RPhone">
              <el-input v-model="borrow.RPhone" disabled></el-input>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row :gutter="5">
          <el-col :span="12">
            <el-form-item label="借出时间：" prop="BbLendtime">
              <el-input v-model="borrow.BbLendtime" disabled></el-input>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="归还时间：" prop="BbReturntime">
              <el-input v-model="borrow.BbReturntime" disabled></el-input>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row :gutter="2">
          <el-col :span="12">
            <el-form-item label-width="130px" label="实际还书时间：" prop="BbRealReturntime">
              <el-input v-model="borrow.BbRealReturntime" disabled></el-input>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row :gutter="5">
          <el-col :span="12">
            <el-form-item label="是否续借：" prop="BbIsreborrow">
              <template>
                <el-radio-group v-model="borrow.BbIsreborrow" disabled>
                  <el-radio :label="1">否</el-radio>
                  <el-radio :label="2">是</el-radio>
                </el-radio-group>
              </template>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="续借天数：" prop="BbReborrowDays">
              <el-input v-model="borrow.BbReborrowDays" disabled></el-input>
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="detailFlag = false">关闭</el-button>
      </div>
    </el-dialog>
  </el-card>
</template>

<script>
import {
  getBorrowList,
  AddBorrow,
  editBorrow,
  Renewborrow,
} from "../api/borrow";
import request from "../utils/request";

export default {
  data() {
    return {
      loading: false,
      readValue: "",
      addFlag: false,
      returnFlag: false,
      contFlag: false,
      detailFlag: false,
      tableData: [],
      pager: {
        page: 1,
        size: 5,
        total: 0,
        RName: "",
      },
      borrow: {
        BbId: "",
        BiName: "",
        RName: "",
        BbLendtime: "",
        BbReturntime: "",
        BbRealReturntime: "",
        BbIsreborrow: "",
        BbReborrowDays: "",
        BiIsbn: "",
        RPhone: "",
        BiNum: "",
        BbIsrenewBook: "",
      },
      rules: {
        RName: [
          { required: true, message: '请输入读者名', trigger: 'blur' },
          { min: 2, max: 15, message: '长度在 2 到 15 个字符', trigger: 'blur' }
        ],
        BiName: [
          { required: true, message: '请输入书籍名', trigger: 'blur' },
          { min: 1, max: 20, message: '长度在 1 到 20 个字符', trigger: 'blur' }
        ],
        BiIsbn: [
          { required: true, message: '请输入书籍isbn', trigger: 'blur' },
          { min: 1, max: 20, message: '长度在 1 到 20 个字符', trigger: 'blur' }
        ],
        RPhone: [
          { required: true, message: '请输入手机号码', trigger: 'blur' },
          { min: 11, max: 11, message: '请输入11位手机号码', trigger: 'blur' },
          {
            pattern: /^(13[0-9]|14[579]|15[0-3,5-9]|16[6]|17[0135678]|18[0-9]|19[89])\d{8}$/,
            message: '请输入正确的手机号码'
          }
        ],
        BbReturntime: [
          { required: true, message: '请选择时间', trigger: 'change' }
        ],
        BbReborrowDays: [
          { required: true, message: '请输入续借天数' },
          { type: 'number', message: '天数必须为数字值' },
        ],
      },
      fine: {
        value: ''
      },
      Isbns: [{
        Isbn: ''
      }],
      Phones: [{
        Phone: ''
      }],
    };

  },
  methods: {
    reload() {
      this.loading = true;
      setTimeout(() => this.loading = false, 500);
      this.listPage();
    },
    openAddDialog() {
      this.addFlag = !this.addFlag;
      this.borrow = {
        BiName: "",
        RName: "",
        BbReturntime: "",
        BiIsbn: "",
        RPhone: "",
      };
    },
    openEditDialog(row) {
      this.returnFlag = !this.returnFlag;
      this.borrow = row;
      request({
        method: 'POST',
        url: '/Borrow/Fine',
        data: { BbRealReturntime: new Date() ,Borrow: this.borrow}
      }).then((res) => {
        if (res.data.code === 200) {
          this.$message({
            message: res.data.msg,
            duration: 1500,
            type: "success",
          });
          this.fine.value = res.data.data;
        }
      }).catch((error) => {
        this.$message.error("出错了,刷新试试！！！");
      });
      
    },
    //续借
    openRenewDialog(row) {
      this.contFlag = !this.contFlag;
      this.borrow = row;
    },
    //详情
    openDetails(row) {
      this.detailFlag = !this.detailFlag;
      this.borrow = row;
    },
    handleCurrentChange(val) {
      this.pager.page = val;
      this.listPage();
    },
    handleSizeChange(val) {
      this.pager.size = val;
      this.listPage();
    },
    selectionIsbns(event) {
      this.Isbns = [];
      request({
        method: 'POST',
        url: '/Borrow/selectionIsbns',
        data: { Name: event.target.value }
      }).then((res) => {
        if (res.data.code === 200) {
          let t = res.data.data;
          for (let i = 0; i < t.length; i++) {
            this.Isbns.push({ Isbn: t[i] })
          }
        }else{
          this.$message({
                  message: res.data.msg,
                  type: "error",
                });
        }
      }).catch((error) => {
        this.$message.error("出错了,刷新试试！！！");
      });
    },
    selectionPhones(event) {
      this.Phones = [];
      request({
        method: 'POST',
        url: '/Borrow/selectionPhones',
        data: { Name: event.target.value }
      }).then((res) => {
        if (res.data.code === 200) {
          let t = res.data.data;
          for (let i = 0; i < t.length; i++) {
            this.Phones.push({ Phone: t[i] })
          }
        }else{
          this.$message({
                  message: res.data.msg,
                  type: "error",
                });
        }
      }).catch((error) => {
        this.$message.error("出错了,刷新试试！！！");
      });
    },
    listPage() {
      getBorrowList({
        page: this.pager.page,
        size: this.pager.size,
        RName: this.pager.RName,
      })
        .then((res) => {
          if (res.data.code === 200) {
            console.log(res.data.data);
            this.tableData = res.data.data;
            this.pager.total = res.data.count;
          }
        })
        .catch((error) => {
          this.$message.error("出错了,刷新试试！！！");
        });
    },
    save() {
      this.$refs["borrow"].validate((valid) => {
        if (valid) {
          AddBorrow(this.borrow)
            .then((res) => {
              if (res.data.code === 200) {
                this.$message({
                  message: res.data.msg,
                  type: "success",
                });
                this.listPage();
                this.addFlag = !this.addFlag; //关闭添加对话框
                this.borrow = {
                  BiName: "",
                  RName: "",
                  BbLendtime: "",
                  BbReturntime: "",
                  BiIsbn: "",
                  RPhone: "",
                };
              } else {
                this.$message({
                  message: res.data.msg,
                  type: "error",
                });
              }
            })
            .catch((error) => {
              this.$message.error(res.data.msg);
            });
        }
      });
    },
    //还书
    edit1() {
      this.$refs["borrow"].validate((valid) => {
        if (valid) {
          editBorrow(this.borrow)
            .then((res) => {
              if (res.data.code === 200) {
                this.$message({
                  message: res.data.msg,
                  type: "success",
                });
                this.listPage();
                this.returnFlag = !this.returnFlag; //关闭添加对话框
                this.borrow = {
                  BbId: "",
                  BiName: "",
                  RName: "",
                  BbLendtime: "",
                  BbReturntime: "",
                  BbRealReturntime: "",
                  BbIsreborrow: "",
                  BbReborrowDays: "",
                  BiIsbn: "",
                  RPhone: "",
                };
              } else {
                this.$message({
                  message: res.data.msg,
                  type: "error",
                });
              }
            })
            .catch((error) => {
              this.$message.error(res.data.msg);
            });
        }
      });
    },
    //续借
    edit2() {
      this.$refs["borrow"].validate((valid) => {
        if (valid) {
          Renewborrow(this.borrow)
            .then((res) => {
              if (res.data.code === 200) {
                this.$message({
                  message: res.data.msg,
                  type: "success",
                });
                this.listPage();
                this.contFlag = !this.contFlag; //关闭添加对话框
                this.borrow = {
                  BbId: "",
                  BiName: "",
                  RName: "",
                  BbLendtime: "",
                  BbReturntime: "",
                  BbRealReturntime: "",
                  BbIsreborrow: "",
                  BbReborrowDays: "",
                  BiIsbn: "",
                  RPhone: "",
                };
              } else {
                this.$message({
                  message: res.data.msg,
                  type: "error",
                });
              }
            })
            .catch((error) => {
              this.$message.error(res.data.msg);
            });
        }
      });
    },
  },

  mounted() {
    this.listPage();
  },
};
</script>

<style scoped>
.el-breadcrumb {
  margin-bottom: 20px;
}

.el-pagination {
  margin-top: 10px;
}
</style>

