<template>
  <v-toolbar>
    <v-btn flat @click="$router.push('/')">Home</v-btn>
    <v-btn flat @click="$router.push('/organisations')">Organisations</v-btn>
    <v-btn flat @click="$router.push('/services')">Services</v-btn>
    <v-spacer></v-spacer>
    <v-toolbar-items>
      <v-btn flat @click="signinClick">Sign in</v-btn>
    </v-toolbar-items>
  </v-toolbar>
</template>

<script setup>
function signinClick() {
  alert('Not implemented yet!');
}
</script>

<style scoped>

</style>
