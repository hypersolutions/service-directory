<template>
  <v-text-field label="Filter" v-model="searchText" @click:clear="onResetFilterClick" @keydown.enter="onFilterClick" clearable>
    <template v-slot:append>
      <v-btn @click="onFilterClick" x-large color="orange" :loading="loading">Filter</v-btn>
    </template>
  </v-text-field>
</template>

<script setup>
const emit = defineEmits(['filterClicked', 'resetFilterClicked']);
defineProps('loading', {default: false});
const searchText = defineModel('searchText', {default: ''});
const loading = defineModel('loading', {default: false});

function onFilterClick() {
  if (searchText.value !== '') {
    emit('filterClicked', {filter: searchText.value});
  }
  else {
    onResetFilterClick();
  }
}

function onResetFilterClick() {
  emit('resetFilterClicked');
}

</script>

<style scoped>

</style>
