<template>
  <section class="account-page">
    <h1 class="account-title">АККАУНТ</h1>

    <div class="account-panels">
      <div class="account-panel">
        <h2 class="panel-title">Войти</h2>

        <form class="account-form" @submit.prevent>
          <div class="form-group">
            <label class="form-label">Email адрес:</label>
            <input class="form-input" type="email" placeholder="email@example.com" />
          </div>

          <div class="form-group">
            <label class="form-label">Пароль:</label>
            <input class="form-input" type="password" placeholder="*********************" />
          </div>

          <div class="form-row">
            <label class="checkbox">
              <input type="checkbox" />
              <span>Запомнить меня</span>
            </label>
            <a class="forgot-link" href="#">Забыли пароль?</a>
          </div>

          <button class="submit-btn-enter" type="submit">ВОЙТИ</button>
        </form>
      </div>

      <div class="account-panel">
        <h2 class="panel-title">Регистрация</h2>

        <form class="account-form" @submit.prevent>
          <div class="form-group">
            <label class="form-label">Email адрес:</label>
            <input class="form-input" type="email" placeholder="email@example.com"/>
          </div>

          <div class="form-group">
            <label class="form-label">Пароль:</label>
            <input class="form-input" type="password" placeholder="*********************"/>
          </div>

          <div class="form-group">
            <label class="form-label">Повторите пароль:</label>
            <input class="form-input" type="password" placeholder="*********************"/>
          </div>

          <button class="submit-btn-reg" type="submit">ЗАРЕГИСТРИРОВАТЬСЯ</button>
        </form>
      </div>
    </div>
  </section>
</template>

<script>
export default {
  name: 'AccountPage'
}
</script>