<!-- eslint-disable -->
<template>
  <div>
    <header>
      <nav class="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
        <div class="container-fluid">
          <a class="navbar-brand">Crawler.WebApp</a>
        </div>
      </nav>
    </header>
    <div>
      <router-view />
    </div>
    <footer class="border-top footer text-muted">
          <div class="container">
              &copy; 2023 - Crawler.WebApp
          </div>
    </footer>
  </div>
</template>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}

nav {
  padding: 30px;
}

nav a {
  font-weight: bold;
  color: #2c3e50;
}

nav a.router-link-exact-active {
  color: #42b983;
}
</style>
