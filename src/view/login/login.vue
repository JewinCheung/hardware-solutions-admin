<style lang="less">
  @import './login.less';

</style>

<template>
  <div class="login">
    <div class="login-con">
      <Card icon="log-in" title="欢迎登录" :bordered="false">
        <div class="form-con">
          <login-form @on-success-valid="handleSubmit"></login-form>
          <p class="login-tip">输入工号和密码即可</p>
        </div>
      </Card>
    </div>
  </div>
</template>

<script>
  import LoginForm from '_c/login-form'
import { resData } from '@/libs/tools'
  import {
    mapActions
  } from 'vuex'
  export default {
    components: {
      LoginForm
    },
    methods: {
      ...mapActions([
        'handleLogin',
        'getUserInfo'
      ]),
      handleSubmit({
        account,
        passwd
      }) {
        this.handleLogin({
          account,
          passwd
        }).then(res => {

          resData(
            res,
            res => {
              this.getUserInfo().then(res => {
                console.log(1);
                this.$router.push({
                  name: 'home'
                })
              })

            },
            res => {
                console.log(2);
              this.$Message.error(res.Msg)
            }
          )





        })
      }
    }
  }

</script>

<style>

</style>
