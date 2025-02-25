<template>
  <v-form v-model="isFormValid">
    <v-text-field label="Postcode" v-model="postcode" :rules="[rules.postcode]" clearable>
      <template v-slot:append>
        <v-btn @click="searchClick" x-large color="orange" :loading="searching" :disabled="!isFormValid">Search</v-btn>
      </template>
    </v-text-field>
    <v-slider v-model="distance" color="orange" :min="1" :max="30" :step="1">
      <template v-slot:label>
        <span class="text-h6 font-weight-light pr-1">Distance</span>
        <span class="text-h6 font-weight-light" v-text="distance"></span>
        <span class="text-h6 font-weight-light pl-1">KM</span>
      </template>
    </v-slider>
    <v-select v-model="selectedMaxResults" :items="maxResults" label="Return Results"/>
  </v-form>
</template>

<script setup>
defineProps('searching', {default: true});
const emit = defineEmits(['onSearch']);
const isFormValid = defineModel('isFormValid', {default: false});
const postcode = defineModel('postcode', {default: ''});
const distance = defineModel('distance', {default: 10});
const selectedMaxResults = defineModel('selectedMaxResults', {default: 10});
const maxResults = [5, 10];
const rules = {
  postcode: value => {
    const pattern = /^[a-z]{1,2}\d[a-z\d]?\s*\d[a-z]{2}$/i;
    return pattern.test(value) || 'A valid postcode is required.';
  }
};

function searchClick() {
  emit('onSearch', {
    postcode: postcode.value,
    distance: distance.value,
    maxResults: selectedMaxResults.value
  });
}
</script>

<style scoped>

</style>
