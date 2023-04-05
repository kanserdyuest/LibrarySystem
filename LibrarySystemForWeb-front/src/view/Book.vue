<template>
  <el-card>
    <el-breadcrumb separator="/">
      <el-breadcrumb-item :to="{ path: '/main' }"><i class="el-icon-s-home"></i>首页</el-breadcrumb-item>
      <el-breadcrumb-item>馆藏信息管理</el-breadcrumb-item>
      <el-breadcrumb-item>图书信息管理</el-breadcrumb-item>
    </el-breadcrumb>

    <el-row :gutter="20">
      <el-col :span="3">
        <el-input placeholder="图书名查询" size="small" v-model="pager.BiName" @change="listPage" clearable></el-input>
      </el-col>
      <el-col :span="3">
        <el-input placeholder="作者检索" size="small" v-model="pager.BiAuthor" @change="listPage" clearable></el-input>
      </el-col>
      <el-col :span="3">
        <el-input placeholder="位置信息检索" size="small" v-model="pager.BiLocation" @change="listPage"
                  clearable></el-input>
      </el-col>
      <el-col :span="3">
        <el-input placeholder="出版社信息" size="small" v-model="pager.BiPress" @change="listPage" clearable></el-input>
      </el-col>
      <el-col :span="3">
        <el-input placeholder="ISBN信息" size="small" v-model="pager.BiIsbn" @change="listPage" clearable></el-input>
      </el-col>
      <el-col :span="3">
        <el-select v-model="pager.BtId" clearable size="small" filterable placeholder="请选择类别" @change="listPage">
          <el-option v-for="item in allType" :key="item.BtId" :label="item.BtName" :value="item.BtId">
          </el-option>
        </el-select>
      </el-col>
      <el-col :span="3">
        <el-select v-model="pager.Deleted" clearable size="small" filterable placeholder="请选择状态"
                   @change="listPage">
          <el-option label="上架中" value="0"></el-option>
          <el-option label="已下架" value="1"></el-option>
        </el-select>
      </el-col>
      <el-col :span="3">
        <el-button type="primary" icon="el-icon-plus" size="small" @click="openAddDialog">图书入库</el-button>
      </el-col>
      <el-tooltip content="刷新" placement="left">
        <el-button icon="el-icon-refresh" size="small" circle style="position: absolute;right:20px;"
                   @click="reload"></el-button>
      </el-tooltip>
    </el-row>

    <el-table :data="tableData" style="width: 100%" v-loading="loading">
      <el-table-column prop="BiName" label="书名" align="center">
      </el-table-column>
      <el-table-column prop="BtName" label="类别" align="center">
      </el-table-column>
      <el-table-column prop="BiPress" label="出版社" align="center">
      </el-table-column>
      <el-table-column prop="BiIsbn" label="ISBN" align="center">
      </el-table-column>
      <el-table-column prop="BiAuthor" label="作者" align="center">
      </el-table-column>
      <el-table-column prop="BiLocation" label="位置" align="center">
      </el-table-column>
      <el-table-column prop="BiPrice" sortable label="价格" align="center">
      </el-table-column>
      <el-table-column prop="BiPages" sortable label="页数" align="center">
      </el-table-column>
      <el-table-column prop="BiAddtime" sortable label="入库时间" align="center">
      </el-table-column>
      <el-table-column prop="BiNum" sortable label="数量" align="center">
        <template v-slot="scope">
          <span v-if="scope.row.BiNum == 0">该图书已全部借出</span>
          <span v-if="scope.row.BiNum != 0">{{ scope.row.BiNum }}</span>
        </template>
      </el-table-column>
      <el-table-column prop="BiCoverPicture" label="封面图片" align="center">
        <template v-slot="scope">
          <el-popover placement="left" :title=scope.row.BiName width="100%" trigger="click">
            <el-image :src=scope.row.BiCoverPicture fit="contain" style="width: 600px;"></el-image>
            <el-image slot="reference" style="width: 100%; height: 100px;cursor: pointer;" :src=scope.row.BiCoverPicture
                      fit="contain">
            </el-image>
          </el-popover>
        </template>
      </el-table-column>
      <el-table-column prop="Deleted" label="在架状态" align="center">
        <template v-slot="s">
          <span v-if="s.row.Deleted === 0">在架</span>
          <span v-if="s.row.Deleted === 1">下架</span>
        </template>
      </el-table-column>
      <el-table-column label="操作" width="180px" align="center">
        <template v-slot="s">
          <el-tooltip content="点击修改" placement="top">
            <el-button type="warning" icon="el-icon-edit" circle @click="openEditDialog(s.row)"></el-button>
          </el-tooltip>
          <el-tooltip content="点击下架" placement="top">
            <el-button v-if="s.row.Deleted === 0" type="danger" icon="el-icon-download" circle
                       @click="openRemoveDialog(s.row)"></el-button>
          </el-tooltip>
          <el-tooltip content="点击上架" placement="top">
            <el-button v-if="s.row.Deleted === 1" type="primary" icon="el-icon-upload2" circle
                       @click="openUpDialog(s.row)"></el-button>
          </el-tooltip>
        </template>
      </el-table-column>
    </el-table>
    <el-pagination @size-change="handleSizeChange" @current-change="handleCurrentChange" :current-page="pager.page"
                   :page-sizes="[5, 10, 15, 20]" :page-size="pager.size"
                   layout="total, sizes, prev, pager, next, jumper"
                   :total="pager.total">
    </el-pagination>

    <el-dialog title="图书信息录入" :visible.sync="addFlag" :show-close="false" width="800px"
               :close-on-click-modal="false">
      <el-form :model="book" ref="book" label-width="100px" :rules="rules">
        <el-row>
          <el-col :span="10">
            <el-form-item label="书名：" prop="BiName">
              <el-input v-model="book.BiName"></el-input>
            </el-form-item>
          </el-col>
          <el-col :span="10">
            <el-form-item label="类别：" prop="UPwd">
              <template>
                <el-select v-model="book.BtId" filterable placeholder="请选择">
                  <el-option v-for="item in allType" :key="item.BtId" :label="item.BtName" :value="item.BtId">
                  </el-option>
                </el-select>
              </template>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="8">
            <el-form-item label="出版社：" prop="BiPress">
              <el-input v-model="book.BiPress"></el-input>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="ISBN:" prop="BiIsbn">
              <el-input v-model="book.BiIsbn"></el-input>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="作者：" prop="BiAuthor">
              <el-input v-model="book.BiAuthor"></el-input>
            </el-form-item>
          </el-col>
        </el-row>

        <el-row>
          <el-col :span="8">
            <el-form-item label="位置：" prop="BiLocation">
              <el-input v-model="book.BiLocation"></el-input>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="价格：" prop="BiPrice">
              <el-input type="number" v-model="book.BiPrice"></el-input>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="页数：" prop="BiPages">
              <el-input type="number" onkeyup="value=value.replace(/^(0+)|[^\d]+/g,'')"
                        v-model="book.BiPages"></el-input>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="8">
            <el-form-item label="数量：" prop="BiNum">
              <el-input type="number" onkeyup="value=value.replace(/^(0+)|[^\d]+/g,'')" v-model="book.BiNum"></el-input>
            </el-form-item>
          </el-col>
        </el-row>
        <input v-model="book.BiCoverPicture" hidden/>
        <el-image v-if="book.BiCoverPicture != null" :src="book.BiCoverPicture"
                  style="height: 200px;width: 300px;"></el-image>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="addFlag = false">关闭</el-button>
        <el-button type="primary" @click="save">保存</el-button>
      </div>
      <div>
        <el-upload ref="uploadfiles" class="upload-demo" action="http://localhost:3741/Book/UploadPic"
                   :on-preview="handlePreview" :on-remove="handleRemove" :file-list="fileList" :on-change="fileChange"
                   :on-success="uploadPhotoAfter" list-type="picture">
          <el-button size="small" type="primary">点击上传首图</el-button>
          <div slot="tip" class="el-upload__tip">只能上传jpg/png文件</div>
        </el-upload>
      </div>
    </el-dialog>

    <el-dialog title="图书信息修改" :visible.sync="editFlag" :show-close="false" width="800px"
               :close-on-click-modal="false">
      <el-form :model="book" ref="book" label-width="100px" :rules="rules">
        <el-row>
          <el-col :span="10">
            <el-form-item label="书名：" prop="BiName">
              <el-input v-model="book.BiName"></el-input>
            </el-form-item>
          </el-col>
          <el-col :span="10">
            <el-form-item label="类别：" prop="BtId">
              <template>
                <el-select v-model="book.BtId" clearable filterable placeholder="请选择">
                  <el-option v-for="item in allType" :key="item.BtId" :label="item.BtName" :value="item.BtId">
                  </el-option>
                </el-select>
              </template>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="8">
            <el-form-item label="出版社：" prop="BiPress">
              <el-input v-model="book.BiPress"></el-input>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="ISBN:" prop="BiIsbn">
              <el-input v-model="book.BiIsbn"></el-input>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="作者：" prop="BiAuthor">
              <el-input v-model="book.BiAuthor"></el-input>
            </el-form-item>
          </el-col>
        </el-row>

        <el-row>
          <el-col :span="8">
            <el-form-item label="位置：" prop="BiLocation">
              <el-input v-model="book.BiLocation"></el-input>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="价格：" prop="BiPrice">
              <el-input type="number" v-model="book.BiPrice"></el-input>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="页数：" prop="BiPages">
              <el-input type="number" onkeyup="value=value.replace(/^(0+)|[^\d]+/g,'')"
                        v-model="book.BiPages"></el-input>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="8">
            <el-form-item label="数量：" prop="BiNum">
              <el-input type="number" onkeyup="value=value.replace(/^(0+)|[^\d]+/g,'')" v-model="book.BiNum"></el-input>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="入库时间：" prop="BiAddtime">
              <template>
                <div class="block">
                  <el-date-picker v-model="book.BiAddtime" type="date" placeholder="选择日期">
                  </el-date-picker>
                </div>
              </template>
            </el-form-item>
          </el-col>
        </el-row>
        <input v-model="book.BiCoverPicture" hidden/>
        <el-image :src="book.BiCoverPicture" style="height: 200px;width: 300px;"></el-image>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="editFlag = false">关闭</el-button>
        <el-button type="primary" @click="edit">保存</el-button>
      </div>
      <div>
        <el-upload ref="uploadfiles" class="upload-demo" action="http://localhost:3741/Book/UploadPic"
                   :on-preview="handlePreview" :on-remove="handleRemove" :file-list="fileList" :on-change="fileChange"
                   :on-success="uploadPhotoAfter" list-type="picture">
          <el-button size="small" type="primary">点击上传首图</el-button>
          <div slot="tip" class="el-upload__tip">只能上传jpg/png文件</div>
        </el-upload>
      </div>
    </el-dialog>

  </el-card>
</template>

<script>
import {getAllTypeList} from "../api/type";
import {getBookList, removeBook, saveBook, editBook, upInfo} from "../api/book";

export default {
  data() {
    return {
      fileList: [],
      rules: {
        BiName: [{
          required: true,
          message: '请输入书名',
          trigger: 'change'
        }],
        BtId: [{
          required: true,
          message: '选择类别',
          trigger: 'change'
        }],
        BiPress: [{
          required: true,
          message: '请输入出版社信息',
          trigger: 'change'
        }],
        BiIsbn: [{
          required: true,
          message: '请输入ISBN',
          trigger: 'change'
        }],
        BiAuthor: [{
          required: true,
          message: '请输入作者信息',
          trigger: 'change'
        }],
        BiLocation: [{
          required: true,
          message: '请输入位置',
          trigger: 'change'
        }],
        BiPages: [{
          required: true,
          message: '请输入页码数',
          trigger: 'change'
        }],
        BiNum: [{
          required: true,
          message: '请输入数量',
          trigger: 'change'
        }],
        BiPrice: [{
          required: true,
          message: '请输入价格',
          trigger: 'change'
        }]
      },
      readValue: '',
      addFlag: false,
      editFlag: false,
      tableData: [],
      loading: false,
      pager: {
        page: 1,
        size: 5,
        total: 0,
        BiName: '',
        BtId: null,
        BiAuthor: '',
        BiLocation: '',
        BiPress: '',
        BiIsbn: '',
        Deleted: null
      },
      book: {
        BiId: 0,
        BiName: '',
        BtId: '',
        BtName: '',
        BiPress: '',
        BiIsbn: '',
        BiAuthor: '',
        BiLocation: '',
        BiPrice: '',
        BiPages: '',
        BiAddtime: '',
        BiNum: '',
        BiCoverPicture: '',
        BiBorrowNum: '',
        Deleted: ''
      },
      allType: [],
    }
  },
  methods: {
    reload() {
      this.loading = true;
      setTimeout(() => this.loading = false, 500);
      this.listPage();
    },
    handleRemove(file, fileList) {
      console.log(file, fileList);
    },

    handlePreview(file) {
      console.log(file);
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
    uploadPhotoAfter(response, file, fileList) {
      this.book.BiCoverPicture = response.data;
      this.$refs.uploadfiles.clearFiles();
    },
    fileChange(file, fileList) {
      if (fileList.length > 0) {
        this.fileList = [fileList[fileList.length - 1]]
      }
    },
    openAddDialog() {
      this.addFlag = !this.addFlag;
      this.book = {
        book: {
          BiId: 0,
          BiName: '',
          BtId: '',
          BiPress: '',
          BiIsbn: '',
          BiAuthor: '',
          BiLocation: '',
          BiPrice: '',
          BiPages: '',
          BiNum: '',
          BiCoverPicture: '',
          BiBorrowNum: '',
          Deleted: ''
        }
      }
    },
    openEditDialog(row) {
      this.editFlag = !this.editFlag;
      this.book = row;
    },
    listPage() {
      getBookList({
        page: this.pager.page,
        size: this.pager.size,
        BiName: this.pager.BiName,
        BtId: this.pager.BtId,
        BiAuthor: this.pager.BiAuthor,
        BiLocation: this.pager.BiLocation,
        BiPress: this.pager.BiPress,
        BiIsbn: this.pager.BiIsbn,
        Deleted: this.pager.Deleted
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
      this.$confirm('此操作将下架该书籍, 是否继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        this.remove(row);
      }).catch(() => {
        this.$message({
          type: 'info',
          message: '已取消'
        });
      });
    },
    remove(row) {
      removeBook(row.BiId).then(res => {
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


    openUpDialog(row) {
      this.$confirm('此操作将上架该书籍, 是否继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        upInfo(row.BiId).then(res => {
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
      }).catch(() => {
        this.$message({
          type: 'info',
          message: '已取消'
        });
      });
    },

    save() {
      this.$refs['book'].validate(valid => {
        if (valid) {
          saveBook(this.book).then(res => {
            if (res.data.code === 200) {
              this.$message({
                message: res.data.msg,
                type: "success"
              })
            }
            this.listPage();
            this.addFlag = !this.addFlag; //关闭添加对话框
            this.book = {
              BiId: 0,
              BiName: '',
              BtId: '',
              BiPress: '',
              BiIsbn: '',
              BiAuthor: '',
              BiLocation: '',
              BiPrice: '',
              BiPages: '',
              BiNum: '',
              BiBorrowNum: '',
              Deleted: ''
            }
          }).catch(error => {
            this.$message.error('网络异常');
          })
        }
      })

    },
    loadAllType() {
      getAllTypeList().then(res => {
        if (res.data.code === 200) {
          let tmp = res.data.data;
          for (let i = 0; i < tmp.length; i++) {
            this.allType.push({BtId: tmp[i].BtId, BtName: tmp[i].BtName});
          }
        }
      })
    },
    edit() {
      editBook(this.book).then(res => {
        if (res.data.code === 200) {
          this.$message({
            message: res.data.msg,
            type: "success"
          })
          this.listPage();
          this.editFlag = !this.editFlag; //关闭修改对话框
          this.book = {
            BiId: 0,
            BiName: '',
            BtId: '',
            BiPress: '',
            BiIsbn: '',
            BiAuthor: '',
            BiLocation: '',
            BiPrice: '',
            BiPages: '',
            BiNum: '',
            BiAddtime: '',
            BiBorrowNum: '',
            Deleted: ''
          }
        }
      }).catch(error => {
        this.$message.error('网络异常');
      })
    }


  },
  mounted() {
    this.listPage();
    this.loadAllType();
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
