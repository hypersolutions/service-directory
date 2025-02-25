<template>
  <v-row align-content="center" class="fill-height" justify="center">
    <v-col class="text-subtitle-1 text-center" justify="center" cols="12">
      {{ message }}
    </v-col>
    <v-col cols="6">
      <v-progress-linear color="orange-accent-4" height="6" indeterminate rounded/>
    </v-col>
  </v-row>
</template>

<script setup>
defineProps(['message']);
</script>

<style scoped>

</style>
