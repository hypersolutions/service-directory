<template>
  <v-card class="mx-auto" max-width="600">
    <v-list-item one-line>
      <v-list-item-content>
        <v-list-item-title class="text-h5">{{ item.name }}</v-list-item-title>
      </v-list-item-content>
    </v-list-item>
    <v-card-text>
      <v-row align="center">
        <v-col cols="12">
          <v-text>{{ item.description }}</v-text>
        </v-col>
      </v-row>
    </v-card-text>
    <v-divider/>
    <v-card-actions class="justify-end">
      <v-btn text @click="onItemClick(item.id)">
        Details
      </v-btn>
    </v-card-actions>
  </v-card>
  <v-divider/>
</template>

<script setup>
const emit = defineEmits(['itemClicked']);
defineProps(['item']);

function onItemClick(itemId) {
  emit('itemClicked', {id: itemId});
}

</script>

<style scoped>

</style>
